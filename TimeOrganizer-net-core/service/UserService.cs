using System.Security.Authentication;
using QRCoder;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TimeOrganizer_net_core.exception;
using TimeOrganizer_net_core.helper;
using TimeOrganizer_net_core.model.DTO;
using TimeOrganizer_net_core.model.DTO.request.user;
using TimeOrganizer_net_core.model.DTO.response.user;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.security;

namespace TimeOrganizer_net_core.service;

public interface IUserService
{
    Task<ServiceResult<TwoFactorAuthResponse>> RegisterUserAsync(RegistrationRequest registration);
    Task<ServiceResult<LoginResponse>> LoginUserAsync(LoginRequest loginRequest);
    Task<ServiceResult> ValidateTwoFactorAuthForLoginAsync(TwoFactorAuthLoginRequest request);
    Task Logout(HttpContext context);
    Task<ServiceResult> ConfirmEmail(string? email, string? code);
    Task<ServiceResult> ForgotPassword(string email);
    Task<ServiceResult> ResetPassword(ResetPasswordRequest request);
    Task<ServiceResult> ChangeEmailAsync(ChangeEmailRequest request);
    Task<ServiceResult> ChangePasswordAsync(ChangePasswordRequest request);
    Task<ServiceResult> ToggleTwoFactorAuthAsync(VerifyUserRequest request);
    Task<ServiceResult> DeleteUserAccountAsync(VerifyUserRequest request);
    Task<ServiceResult> ChangeCurrentLocaleAsync(AvailableLocales locale);
    Task<bool> GetTwoFactorAuthStatusAsync();
    Task<UserResponse> GetLoggedUserDataAsync();
    Task<string?> GetTwoFactorAuthQrCodeAsync(User? user = null);
    Task<IEnumerable<string>?> GenerateTwoFactorAuthScratchCodesAsync(User? user = null);
}

public class UserService(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    ILoggedUserService loggedUserService,
    IGoogleRecaptchaService googleRecaptchaService,
    IEmailService emailService,
    ITaskUrgencyService taskUrgencyService,
    IRoutineTimePeriodService routineTimePeriodService,
    IRoleService roleService,
    IMapper mapper,
    IConfiguration configuration) : IUserService
{
    public async Task<ServiceResult<TwoFactorAuthResponse>> RegisterUserAsync(RegistrationRequest registration)
    {
        if (!await googleRecaptchaService.VerifyRecaptchaAsync(registration.RecaptchaToken, "register"))
        {
            return ServiceResult<TwoFactorAuthResponse>.Error(
                ServiceResultErrorType.BadRequest,
                "Wrong captcha token  or action"
            );
        }

        var newUser = new User
        {
            Email = registration.Email,
            UserName = registration.Email,
            TwoFactorEnabled = registration.TwoFactorEnabled,
            CurrentLocale = registration.CurrentLocale,
            Timezone = TimeZoneInfo.FindSystemTimeZoneById(registration.Timezone),
        };
        var result = await userManager.CreateAsync(newUser, registration.Password);
        if (!result.Succeeded)
        {
            return result.Errors.Any(e => e.Code is "DuplicateUserName" or "DuplicateEmail")
                ? ServiceResult<TwoFactorAuthResponse>.Error(ServiceResultErrorType.Conflict,
                    "User already exists with EMAIL: " + newUser.Email)
                : ServiceResult<TwoFactorAuthResponse>.Error(ServiceResultErrorType.BadRequest,
                    "Failed to register user because: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        await SetDefaultSettingsAsync(newUser.Id);

        if (!newUser.TwoFactorEnabled)
        {
            return ServiceResult<TwoFactorAuthResponse>.Successful(
                new TwoFactorAuthResponse
                {
                    TwoFactorEnabled = false,
                });
        }
        return ServiceResult<TwoFactorAuthResponse>.Successful(
            new TwoFactorAuthResponse
            {
                TwoFactorEnabled = newUser.TwoFactorEnabled,
                QrCode = await GetTwoFactorAuthQrCodeAsync(newUser),
                RecoveryCodes = await GenerateTwoFactorAuthScratchCodesAsync(newUser)
            });
    }

    public async Task<ServiceResult<LoginResponse>> LoginUserAsync(LoginRequest loginRequest)
    {
        if (!await googleRecaptchaService.VerifyRecaptchaAsync(loginRequest.RecaptchaToken, "login"))
        {
            return ServiceResult<LoginResponse>.Error(
                ServiceResultErrorType.BadRequest,
                "Wrong captcha token or action"
            );
        }

        var userResult = await GetByEmailAsync(loginRequest.Email);
        if (!userResult.Success)
        {
            return ServiceResult<LoginResponse>.Error(userResult.ErrorType, userResult.ErrorMessage);
        }

        var user = userResult.Data;
        var result = await signInManager.PasswordSignInAsync(user, loginRequest.Password,
            loginRequest.StayLoggedIn, true);
     

        if (result.IsLockedOut)
        {
            var lockoutDuration = user.LockoutEnd!.Value - DateTimeOffset.Now;
            var minutes = (int)lockoutDuration.TotalMinutes;
            var seconds = lockoutDuration.Seconds;
            return ServiceResult<LoginResponse>.Error(ServiceResultErrorType.UserLockedOut,
                $"User locked out for {minutes}m {seconds}s");
        }

        if (result.IsNotAllowed)
        {
            await userManager.AccessFailedAsync(user);
            return ServiceResult<LoginResponse>.Error(ServiceResultErrorType.AuthenticationFailed,
                "Wrong email or password");
        }
        if (result is { Succeeded: false, RequiresTwoFactor: false })
        {
            return ServiceResult<LoginResponse>.Error(ServiceResultErrorType.InternalServerError, result.ToString());
        }
        user.Timezone = TimeZoneInfo.FindSystemTimeZoneById(loginRequest.Timezone);
        await userManager.UpdateAsync(user);
        return ServiceResult<LoginResponse>.Successful(
            new LoginResponse
            {
                Email = loginRequest.Email,
                RequiresTwoFactor = result.RequiresTwoFactor,
                CurrentLocale = user.CurrentLocale
            });
    }

    public async Task<ServiceResult> ValidateTwoFactorAuthForLoginAsync(TwoFactorAuthLoginRequest request)
    {
        var result =
            await signInManager.TwoFactorAuthenticatorSignInAsync(request.Token, request.StayLoggedIn,
                false);
        if (!result.Succeeded)
        {
            ServiceResult.Error(ServiceResultErrorType.InternalServerError, result.ToString());
        }
        return ServiceResult.Successful();
    }

    public async Task Logout(HttpContext context)
    {
        await signInManager.SignOutAsync();
    }

    //TODO email sender and test these methods

    #region emailSenderNeeded

    public async Task<ServiceResult> ForgotPassword(string email)
    {
        var userResult = await GetByEmailAsync(email);
        if (!userResult.Success)
        {
            return ServiceResult.Error(userResult.ErrorType, userResult.ErrorMessage);
        }

        if (!await userManager.IsEmailConfirmedAsync(userResult.Data))
        {
            return ServiceResult.Error(ServiceResultErrorType.EmailNotConfirmed, "User doesn't have email confirmed");
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(userResult.Data);
        await emailService.SendEmailAsync(
            email,
            $"Password reset - {configuration["App:Name"]}",
            emailService.GenerateForgottenPasswordEmail(token));
        return ServiceResult.Successful();
    }

    public async Task<ServiceResult> ResetPassword(ResetPasswordRequest request)
    {
        var userResult = await GetByEmailAsync(request.Email);
        if (!userResult.Success)
        {
            return ServiceResult.Error(userResult.ErrorType, userResult.ErrorMessage);
        }

        var result = await userManager.ResetPasswordAsync(userResult.Data, request.ResetCode, request.NewPassword);
        return result.Succeeded
            ? ServiceResult.Successful()
            : ServiceResult.Error(ServiceResultErrorType.IdentityError, result.Errors.ToString());
    }

    public async Task<ServiceResult> ConfirmEmail(string? email, string? code)
    {
        if (email == null || code == null)
        {
            return ServiceResult.Error(ServiceResultErrorType.BadRequest, "UserId and Code must be supplied");
        }

        var userResult = await GetByEmailAsync(email);
        if (!userResult.Success)
        {
            return ServiceResult.Error(userResult.ErrorType, userResult.ErrorMessage);
        }

        var result = await userManager.ConfirmEmailAsync(userResult.Data, code);
        return result.Succeeded
            ? ServiceResult.Successful()
            : ServiceResult.Error(ServiceResultErrorType.IdentityError, result.Errors.ToString());
    }

    #endregion

    public async Task<ServiceResult> ChangeCurrentLocaleAsync(AvailableLocales locale)
    {
        var user = await GetLoggedUserAsync();
        user.CurrentLocale = locale;
        var result = await userManager.UpdateAsync(user);
        return result.Succeeded
            ? ServiceResult.Successful()
            : ServiceResult.Error(ServiceResultErrorType.IdentityError, result.Errors.ToString());
    }


    public async Task<bool> GetTwoFactorAuthStatusAsync()
    {
        return loggedUserService.GetLoggedUserTwoFactorAuthStatus() ?? (await GetLoggedUserAsync()).TwoFactorEnabled;
    }

    private async Task<bool> ValidateTwoFactorAuthAsync(string token)
    {
        return await userManager.VerifyTwoFactorTokenAsync(await GetLoggedUserAsync(),
            TokenOptions.DefaultAuthenticatorProvider, token);
    }
    public async Task<ServiceResult> ChangeEmailAsync(ChangeEmailRequest request)
    {
        var user = await GetLoggedUserAsync();
        IdentityResult? result;
        if (user.TwoFactorEnabled)
        {
            if (string.IsNullOrEmpty(request.TwoFactorAuthToken))
            {
                return ServiceResult.Error(ServiceResultErrorType.TwoFactorAuthRequired,
                    "Two-factor authentication is required to proceed.");
            }

            var isTokenValid = await ValidateTwoFactorAuthAsync(request.TwoFactorAuthToken);
            if (!isTokenValid)
            {
                return ServiceResult.Error(ServiceResultErrorType.InvalidTwoFactorAuthToken,
                    "Invalid two-factor authentication token.");
            }
            // result = await userManager.ChangeEmailAsync(user, request.NewEmail, await userManager.GenerateChangeEmailTokenAsync(user,request.NewEmail));
        }
        else
        {   
            //Bez 2fa treba poslat email s tokenom a ten overit spravi takisto aj pri hesle
            // result = await userManager.ChangeEmailAsync(user, request.NewEmail, request.TwoFactorAuthToken);
        }
        result = await userManager.ChangeEmailAsync(user, request.NewEmail, await userManager.GenerateChangeEmailTokenAsync(user,request.NewEmail));
        if (!result.Succeeded)
        {
            return ServiceResult.Error(ServiceResultErrorType.IdentityError, result.Errors.ToString());
        }
        await userManager.UpdateSecurityStampAsync(user);
        await signInManager.SignOutAsync();
        return ServiceResult.Successful();
    }
    public async Task<ServiceResult> ChangePasswordAsync(ChangePasswordRequest request)
    {
        var user = await GetLoggedUserAsync();
        if (user.TwoFactorEnabled)
        {
            if (string.IsNullOrEmpty(request.TwoFactorAuthToken))
            {
                return ServiceResult.Error(ServiceResultErrorType.TwoFactorAuthRequired,
                    "Two-factor authentication is required to proceed.");
            }

            var isTokenValid = await ValidateTwoFactorAuthAsync(request.TwoFactorAuthToken);
            if (!isTokenValid)
            {
                return ServiceResult.Error(ServiceResultErrorType.InvalidTwoFactorAuthToken,
                    "Invalid two-factor authentication token.");
            }
        }
        var result = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!result.Succeeded)
        {
            return ServiceResult.Error(ServiceResultErrorType.IdentityError, result.Errors.ToString());
        }
        await userManager.UpdateSecurityStampAsync(user);
        await signInManager.SignOutAsync();
        return ServiceResult.Successful();
    }

    private async Task<ServiceResult> VerifyUserAsync(VerifyUserRequest request)
    {
        var user = await GetLoggedUserAsync();
        if (user.TwoFactorEnabled)
        {
            if (string.IsNullOrEmpty(request.TwoFactorAuthToken))
            {
                return ServiceResult.Error(ServiceResultErrorType.TwoFactorAuthRequired,
                    "Two-factor authentication is required to proceed");
            }

            var isTokenValid = await ValidateTwoFactorAuthAsync(request.TwoFactorAuthToken);
            if (!isTokenValid)
            {
                return ServiceResult.Error(ServiceResultErrorType.InvalidTwoFactorAuthToken,
                    "Invalid two-factor authentication token");
            }
        }

        var result = await userManager.CheckPasswordAsync(user, request.Password);
        return result
            ? ServiceResult.Successful()
            : ServiceResult.Error(ServiceResultErrorType.AuthenticationFailed, "Wrong password");
    }

    public async Task<ServiceResult> ToggleTwoFactorAuthAsync(VerifyUserRequest request)
    {
        var user = await GetLoggedUserAsync();
        if (!await userManager.CheckPasswordAsync(user, request.Password))
        {
            ServiceResult.Error(ServiceResultErrorType.AuthenticationFailed, "Wrong password");
        }
        if (user.TwoFactorEnabled)
        {
            if (string.IsNullOrEmpty(request.TwoFactorAuthToken))
            {
                return ServiceResult.Error(ServiceResultErrorType.TwoFactorAuthRequired,
                    "Two-factor authentication is required to proceed.");
            }

            var isTokenValid = await ValidateTwoFactorAuthAsync(request.TwoFactorAuthToken);
            if (!isTokenValid)
            {
                return ServiceResult.Error(ServiceResultErrorType.InvalidTwoFactorAuthToken,
                    "Invalid two-factor authentication token.");
            }
        }
        user.TwoFactorEnabled = !user.TwoFactorEnabled;
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return ServiceResult.Error(ServiceResultErrorType.IdentityError, result.Errors.ToString());
        }
        
        if (!user.TwoFactorEnabled)
        {
            return ServiceResult<TwoFactorAuthResponse>.Successful(
                new TwoFactorAuthResponse
                {
                    TwoFactorEnabled = false,
                });
        }
        await userManager.UpdateSecurityStampAsync(user);
        await signInManager.SignOutAsync();
        return ServiceResult<TwoFactorAuthResponse>.Successful(
            new TwoFactorAuthResponse
            {
                TwoFactorEnabled = true,
                QrCode = await GetTwoFactorAuthQrCodeAsync(user),
                RecoveryCodes = await GenerateTwoFactorAuthScratchCodesAsync(user)
            });
    }

    public async Task<ServiceResult> DeleteUserAccountAsync(VerifyUserRequest request)
    {
        var verifyResult = await VerifyUserAsync(request);
        if (!verifyResult.Success)
        {
            return verifyResult;
        }

        var result = await userManager.DeleteAsync(await GetLoggedUserAsync());
        return result.Succeeded
            ? ServiceResult.Successful()
            : ServiceResult.Error(ServiceResultErrorType.IdentityError, result.Errors.ToString());
    }

    public async Task<UserResponse> GetLoggedUserDataAsync()
    {
        var loggedUser = await GetLoggedUserAsync();
        return mapper.Map<UserResponse>(loggedUser);
    }

    public async Task<string?> GetTwoFactorAuthQrCodeAsync(User? user = null)
    {
        user ??= await GetLoggedUserAsync();
        if (!user.TwoFactorEnabled)
        {
            return null;
        }

        var totpAuthenticatorKey = await userManager.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrEmpty(totpAuthenticatorKey))
        {
            await userManager.ResetAuthenticatorKeyAsync(user);
            totpAuthenticatorKey = await userManager.GetAuthenticatorKeyAsync(user);
        }

        return GenerateQrCode(totpAuthenticatorKey!, user.Email!);
    }

    public async Task<IEnumerable<string>?> GenerateTwoFactorAuthScratchCodesAsync(User? user = null)
    {
        user ??= await GetLoggedUserAsync();
        if (!user.TwoFactorEnabled)
        {
            return null;
        }
        return await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 5) ??
               throw new InvalidOperationException("2Fa scratch codes generation failed");
    }

    private async Task SetDefaultSettingsAsync(long userId)
    {
        await taskUrgencyService.CreateDefaultItems(userId);
        await routineTimePeriodService.CreateDefaultItems(userId);
        await roleService.CreateDefaultItems(userId);
    }

    private async Task SendConfirmationEmail(User user)
    {
    }

    private static string GenerateQrCode(string secretKey, string userEmail)
    {
        var appName = Environment.GetEnvironmentVariable("APP_NAME") ??
                      throw new Exception("No environment variable APP_NAME");
        var otpAuthUrl = $"otpauth://totp/{appName}:{userEmail}?secret={secretKey}&issuer={appName}&digits=6";

        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(otpAuthUrl, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new PngByteQRCode(qrCodeData);
        return Convert.ToBase64String(qrCode.GetGraphic(3));
    }

    private async Task<User> GetLoggedUserAsync()
    {
        var principal = loggedUserService.GetLoggedUserPrincipal();
        var user = await userManager.GetUserAsync(principal);
        if (user == null)
        {
            throw new UserByPrincipalNotFoundException(principal);
        }

        return user;
    }
    // protected long GetLoggedUserId()
    // {
    //     return loggedUserService.GetLoggedUserId();
    // }

    // private async Task<User> GetByIdAsync(long id)
    // {
    //     var user = await userManager.FindByIdAsync(id.ToString());
    //     if (user == null)
    //     {
    //         throw new UserNotFoundException(id);
    //     }
    //     return user;
    // }

    private async Task<ServiceResult<User>> GetByEmailAsync(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        return user == null
            ? ServiceResult<User>.Error(ServiceResultErrorType.NotFound, $"User with EMAIL: '{email}' was not found")
            : ServiceResult<User>.Successful(user);
    }
}