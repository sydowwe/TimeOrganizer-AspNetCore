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
    Task<ServiceResult<RegistrationResponse>> RegisterUserAsync(RegistrationRequest registration);
    Task<ServiceResult<LoginResponse>> LoginUserAsync(LoginRequest loginRequest);
    Task<ServiceResult> ValidateTwoFactorAuthLoginAsync(GoogleAuthLoginRequest request);
    Task Logout(HttpContext context);
    Task<ServiceResult> ConfirmEmail(string? email, string? code);
    Task<ServiceResult> ForgotPassword(string email);
    Task<ServiceResult> ResetPassword(ResetPasswordRequest request);
    Task<ServiceResult> ChangePasswordAsync(string currentPassword, string newPassword);
    Task<ServiceResult> ChangeCurrentLocaleAsync(AvailableLocales locale);
    Task<bool> WereSensitiveChangesMadeAsync(UserRequest changedUser);
    Task<bool> ValidatePasswordAndReturnTwoFactorAuthStatusAsync(string password);
    Task<bool> ValidateTwoFactorAuthAsync(string code);
    Task<UserResponse> GetLoggedUserDataAsync();
    Task<EditedUserResponse> EditUserDataAsync(UserRequest request);
    Task<ServiceResult> DeleteUserAccountAsync();
    Task<byte[]?> GetTwoFactorAuthQrCodeAsync(User? user = null);
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
    public async Task<ServiceResult<RegistrationResponse>> RegisterUserAsync(RegistrationRequest registration)
    {
        if (!await googleRecaptchaService.verifyRecaptchaAsync(registration.recaptchaToken, "register"))
        {
            return ServiceResult<RegistrationResponse>.Error(
                ServiceResultErrorType.BadRequest,
                "Wrong captcha token or action"
            );
        }
        var newUser = new User
        {
            name = registration.name,
            surname = registration.surname,
            Email = registration.Email,
            UserName = registration.Email,
            TwoFactorEnabled = registration.TwoFactorEnabled,
            // currentLocale = registration.currentLocale,
            isStayLoggedIn = false
        };
        var result = await userManager.CreateAsync(newUser, registration.password);
        if (!result.Succeeded)
        {
            if (result.Errors.Any(e => e.Code is "DuplicateUserName" or "DuplicateEmail"))
            {
                return ServiceResult<RegistrationResponse>.Error(ServiceResultErrorType.Conflict, "Failed to register user with EMAIL: " + newUser.Email);
            }
            return ServiceResult<RegistrationResponse>.Error(ServiceResultErrorType.BadRequest, "Failed to register user " + string.Join(", ", result.Errors.Select(e => e.Description)));

        }
        var scratchCodes = await GenerateTwoFactorAuthScratchCodesAsync(newUser);
        var qrCode = await GetTwoFactorAuthQrCodeAsync(newUser);
        await SetDefaultSettingsAsync(newUser.Id);
        return ServiceResult<RegistrationResponse>.Successful(
            new RegistrationResponse
            {
                Email = newUser.Email,
                RequiresTwoFactor = newUser.TwoFactorEnabled,
                QrCode = qrCode,
                ScratchCodes = scratchCodes
            });
    }
    public async Task<ServiceResult<LoginResponse>> LoginUserAsync(LoginRequest loginRequest)
    {
        if (!await googleRecaptchaService.verifyRecaptchaAsync(loginRequest.recaptchaToken, "login"))
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
        var result = await signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.password,
            loginRequest.stayLoggedIn, true);
        var user = userResult.Data;
        if (result.IsNotAllowed)
        {
            await userManager.AccessFailedAsync(user);
            return ServiceResult<LoginResponse>.Error(ServiceResultErrorType.AuthenticationFailed,
                "Wrong email or password");
        }
        if (result.IsLockedOut)
        {
            return ServiceResult<LoginResponse>.Error(ServiceResultErrorType.UserLockedOut,
                "User locked out for  minutes");
        }
        if (!result.Succeeded)
        {
            ServiceResult<LoginResponse>.Error(ServiceResultErrorType.InternalServerError, result.ToString());
        }
        user.isStayLoggedIn = loginRequest.stayLoggedIn;
        user.timezone = loginRequest.timezone;
        await userManager.UpdateAsync(user);
        return ServiceResult<LoginResponse>.Successful(
            new LoginResponse
            {
                Email = loginRequest.Email,
                RequiresTwoFactor = result.RequiresTwoFactor,
                CurrentLocale = user.currentLocale
            });
    }

    public async Task<ServiceResult> ValidateTwoFactorAuthLoginAsync(GoogleAuthLoginRequest request)
    {
        var result = await signInManager.TwoFactorAuthenticatorSignInAsync(request.Code,request.StayLoggedIn,request.RememberClient);
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

    public async Task<ServiceResult> ChangePasswordAsync(string currentPassword, string newPassword)
    {
        var user = await GetLoggedUserAsync();
        var result = await userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        return result.Succeeded
            ? ServiceResult.Successful()
            : ServiceResult.Error(ServiceResultErrorType.IdentityError, result.Errors.ToString());
    }

    public async Task<ServiceResult> ChangeCurrentLocaleAsync(AvailableLocales locale)
    {
        var user = await GetLoggedUserAsync();
        user.currentLocale = locale;
        var result = await userManager.UpdateAsync(user);
        return result.Succeeded
            ? ServiceResult.Successful()
            : ServiceResult.Error(ServiceResultErrorType.IdentityError, result.Errors.ToString());
    }

    public async Task<bool> WereSensitiveChangesMadeAsync(UserRequest changedUser)
    {
        var loggedUser = await GetLoggedUserAsync();
        return !(loggedUser.Email!.Equals(changedUser.Email) &&
                 loggedUser.TwoFactorEnabled == changedUser.TwoFactorEnabled);
    }

    public async Task<bool> ValidatePasswordAndReturnTwoFactorAuthStatusAsync(string password)
    {
        var loggedUser = await GetLoggedUserAsync();
        var result = await signInManager.CheckPasswordSignInAsync(loggedUser, password, false);
        if (!result.Succeeded)
        {
            throw new AuthenticationException();
        }
        return loggedUser.TwoFactorEnabled;
    }

    public async Task<bool> ValidateTwoFactorAuthAsync(string code)
    {
        return await userManager.VerifyTwoFactorTokenAsync(await GetLoggedUserAsync(),
            TokenOptions.DefaultAuthenticatorProvider, code);
    }

    public async Task<UserResponse> GetLoggedUserDataAsync()
    {
        var loggedUser = await GetLoggedUserAsync();
        return mapper.Map<UserResponse>(loggedUser);
    }
    //TODO FINISH 
    public async Task<EditedUserResponse> EditUserDataAsync(UserRequest request)
    {
        var loggedUser = await GetLoggedUserAsync();
        byte[]? qrCode = null;
        IEnumerable<string>? scratchCodes = null;

        if (!loggedUser.TwoFactorEnabled && request.TwoFactorEnabled)
        {
            qrCode = await GetTwoFactorAuthQrCodeAsync(loggedUser) ?? throw new InvalidOperationException();
            scratchCodes = await GenerateTwoFactorAuthScratchCodesAsync(loggedUser);
        }
        await signInManager.RefreshSignInAsync(loggedUser);

        mapper.Map(loggedUser, request);
        await userManager.UpdateAsync(loggedUser);
        return new EditedUserResponse(mapper.Map<UserResponse>(loggedUser))
        {
            qrCode = qrCode,
            scratchCodes = scratchCodes
        };
    }


    public async Task<ServiceResult> DeleteUserAccountAsync()
    {
        var result = await userManager.DeleteAsync(await GetLoggedUserAsync());
        return result.Succeeded
            ? ServiceResult.Successful()
            : ServiceResult.Error(ServiceResultErrorType.IdentityError, result.Errors.ToString());
    }

    public async Task<byte[]?> GetTwoFactorAuthQrCodeAsync(User? user = null)
    {
        user ??= await GetLoggedUserAsync();
        if (user.TwoFactorEnabled)
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
        if (user.TwoFactorEnabled)
        {
            return null;
        }

        return await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 5) ??
               throw new InvalidOperationException("2Fa scratch codes generation failed");
    }

    private async Task SetDefaultSettingsAsync(long userId)
    {
        await taskUrgencyService.createDefaultItems(userId);
        await routineTimePeriodService.createDefaultItems(userId);
        await roleService.createDefaultItems(userId);
    }

    private static byte[] GenerateQrCode(string secretKey, string userEmail)
    {
        var appName = Environment.GetEnvironmentVariable("APP_NAME") ??
                      throw new Exception("No environment variable APP_NAME");
        var otpAuthUrl = $"otpauth://totp/{appName}:{userEmail}?secret={secretKey}&issuer={appName}&digits=6";

        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(otpAuthUrl, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new PngByteQRCode(qrCodeData);
        return qrCode.GetGraphic(20);
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
            ? ServiceResult<User>.Error(ServiceResultErrorType.NotFound, $"User with EMAIL: {email} was not found")
            : ServiceResult<User>.Successful(user);
    }
}