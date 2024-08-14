using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.identity;
using Microsoft.AspNetCore.Identity;
using TimeOrganizer_net_core.exception;
using TimeOrganizer_net_core.model.DTO.request.user;
using TimeOrganizer_net_core.model.DTO.response.user;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository;
using TimeOrganizer_net_core.security;

namespace TimeOrganizer_net_core.service;

public interface IUserService
{
}

public class UserService(
    IUserRepository userRepository,
    IPasswordHasher<User> passwordHasher,
    IAuthenticationService authenticationService,
    IJwtService jwtService,
    IGoogleAuthenticator googleAuthenticator,
    IEmailService emailService,
    ITaskUrgencyService taskUrgencyService,
    IRoutineTimePeriodService routineTimePeriodService,
    IRoleService roleService,
    IGoogleService googleService,
    IMapper mapper,
    IConfiguration configuration) : IUserService
{

    public async Task<RegistrationResponse> registerUserAsync(RegistrationRequest registration)
    {
        if (!await googleService.VerifyRecaptchaAsync(registration.recaptchaToken, "register"))
        {
            throw new ReCaptchaException();
        }

        var newUser = new User
        {
            name = registration.name,
            surname = registration.surname,
            email = registration.email,
            password = passwordHasher.HashPassword(null, registration.password), // ASP.NET Core uses PasswordHash property
            role = UserRole.User,
            currentLocale = registration.currentLocale,
            timezone = registration.timezone
        };

        RegistrationResponse response;

        if (registration.has2FA)
        {
            var credentials = googleAuthenticator.CreateCredentials();
            newUser.secretKey2FA = credentials.Key;
            newUser.scratchCodes = credentials.scratchCodes;
            var qrCode = Generate2FaQrCode(newUser.email, credentials);
            response = new RegistrationResponse
            {
                email = newUser.email,
                has2FA = true,
                qrCode = qrCode
            };
        }
        else
        {
            response = new RegistrationResponse
            {
                email = newUser.email,
                has2FA = false
            };
        }

        try
        {
            await userRepository.addAsync(newUser);
            await SetDefaultSettingsAsync(newUser);
        }
        catch (Exception ex)
        {
            throw new Exception(ex);
        }

        return response;
    }

    public async Task<LoginResponse> loginUserAsync(LoginRequest loginRequest)
    {
        if (!await googleService.VerifyRecaptchaAsync(loginRequest.recaptchaToken, "login"))
        {
            throw new ReCaptchaException("wrong captcha");
        }


        var user = await findByEmailAsync(loginRequest.email);
        if (user == null)
        {
            throw new UserNotFoundException(loginRequest.email);
        }

        var result = await authenticationService.AuthenticateAsync(
            new AuthenticationProperties(), 
            new UsernamePasswordAuthenticationToken(loginRequest.email, loginRequest.password));

        if (!result.Succeeded)
        {
            throw new AuthenticationException();
        }


        if (user.StayLoggedIn != loginRequest.stayLoggedIn)
        {
            await userRepository.updateStayLoggedInByIdAsync(user.id, loginRequest.stayLoggedIn);
        }

        if (user.Timezone != loginRequest.timezone)
        {
            await userRepository.updateUserTimezoneAsync(user.id, loginRequest.timezone);
        }

        if (user.has2FA)
        {
            return new LoginResponse
            {
                id = user.id,
                email = loginRequest.email,
                has2FA = true,
                currentLocale = user.currentLocale
            };
        }
        else
        {
            var jwtToken = jwtService.GenerateToken(user.email, user.id, GetTokenExpirationLength(loginRequest.stayLoggedIn));
            return new LoginResponse
            {
                id = user.id,
                token = jwtToken,
                email = loginRequest.email,
                has2FA = false,
                currentLocale = user.currentLocale
            };
        }
    }
    
    // private byte[] Generate2FaQrCode(string email, GoogleAuthenticatorKey key)
    // {
    //     var qrGenerator = new QRCodeGenerator();
    //     var otpAuthUrl = GoogleAuthenticatorQRGenerator.GetOtpAuthTotpURL(
    //         configuration["App:Name"], email, key);
    //     using var qrCodeData = qrGenerator.CreateQrCode(otpAuthUrl, QRCodeGenerator.ECCLevel.Q);
    //     using var qrCode = new Base64QRCode(qrCodeData);
    //     return qrCode.GetGraphic(120);
    // }

    // public async Task<Oauth2LoginResponse> Oauth2LoginUserAsync(OAuth2User oAuth2User)
    // {
    //     var email = oAuth2User.Name;
    //     var user = await FindByemailAsync(email);
    //
    //     if (user.has2FA)
    //     {
    //         // Save secretKey2Fa to session or cache by email
    //         return new Oauth2LoginResponse
    //         {
    //             Id = user.id,
    //             email = email,
    //             has2FA = true,
    //             Authenticated = true
    //         };
    //     }
    //     else
    //     {
    //         var jwtToken = jwtService.GenerateToken(user.email, user.id, GetTokenExpirationLength(user.stayLoggedIn));
    //         return new Oauth2LoginResponse
    //         {
    //             Id = user.id,
    //             Token = jwtToken,
    //             email = email,
    //             Authenticated = true,
    //             has2FA = false
    //         };
    //     }
    // }

    

    public async Task<GoogleAuthResponse> Validate2FALoginAsync(GoogleAuthLoginRequest request)
    {
        var user = await FindByemailAsync(request.email);
        if (user == null)
        {
            throw new NotFoundException($"User with email: {request.email} not found");
        }

        var is2FAValid = googleAuthenticator.Authorize(user.secretKey2FA, int.Parse(request.code));
        var response = new GoogleAuthResponse
        {
            authorized = is2FAValid,
            token = is2FAValid ? jwtService.GenerateToken(user.email, user.id, GetTokenExpirationLength(user.isStayLoggedIn)) : null
        };

        if (is2FAValid)
        {
        }
        else
        {
        }

        return response;
    }

    public void Logout(string token)
    {
        jwtService.InvalidateToken(token);
    }

    public async Task ChangecurrentLocaleAsync(AvailableLocales locale)
    {
        var loggedUserId = GetLoggedUserId();
        await userRepository.updateCurrentLocaleByIdAsync(loggedUserId, locale);
    }

    public async Task ResetPasswordAsync(string email)
    {
        var tempPassword = GenerateTemporaryPassword();
        var updated = await userRepository.updatePasswordByEmailAsync(email, passwordHasher.HashPassword(null, tempPassword));
        if (updated<1)
        {
            throw new InvalidOperationException();
        }
        var emailBody = emailService.GenerateForgottenPasswordemail(tempPassword);
        await emailService.SendemailAsync(email, $"Password reset - {configuration["App:Name"]}", emailBody);
    }

    public async Task<bool> WereSensitiveChangesMadeAsync(UserRequest changedUser)
    {
        var loggedUser = await GetLoggedUserAsync();
        return !(loggedUser.email.Equals(changedUser.email) && loggedUser.has2FA == changedUser.has2FA);
    }

    public async Task<bool> VerifyPasswordAndReturn2FAStatusAsync(string token, string password)
    {
        var loggedUser = await GetLoggedUserAsync(token);
        var result = await authenticationService.AuthenticateAsync(
            new AuthenticationProperties(), 
            new UsernamePasswordAuthenticationToken(loggedUser.email, password));

        if (!result.Succeeded)
        {
            throw new AuthenticationException();
        }

        return loggedUser.has2FA;
    }

    public async Task<bool> Validate2FAAsync(string token, int code)
    {
        var loggedUser = await GetLoggedUserAsync(token);
        return googleAuthenticator.Authorize(loggedUser.secretKey2FA, code);
    }

    public async Task<UserResponse> GetLoggedUserDataAsync()
    {
        var loggedUser = await GetLoggedUserAsync();
        return mapper.Map<UserResponse>(loggedUser);
    }

    public async Task<EditedUserResponse> EditLoggedUserDataAsync(string token, UserRequest request)
    {
        var loggedUser = await GetLoggedUserAsync(token);
        string newToken = null;
        byte[] qrCode = null;

        if (!loggedUser.email.Equals(request.email))
        {
            jwtService.InvalidateToken(token);
            newToken = jwtService.GenerateToken(request.email, loggedUser.id, GetTokenExpirationLength(loggedUser.isStayLoggedIn));
        }

        if (!loggedUser.has2FA() && request.has2FA)
        {
            var credentials = googleAuthenticator.CreateCredentials();
            loggedUser.secretKey2FA = credentials.Key;
            loggedUser.scratchCodes = credentials.scratchCodes;
            qrCode = generate2FaQrCode(loggedUser.email, credentials);
        }

        mapper.Map(loggedUser, request);
        await userRepository.updateAsync(loggedUser);

        return userMapper.ConvertToEditedUserSettingsResponse(loggedUser, newToken, qrCode);
    }

    public async Task ChangeLoggedUserPasswordAsync(string token, string newPassword)
    {
        var loggedUser = await GetLoggedUserAsync(token);
        loggedUser.passwordHash = passwordHasher.HashPassword(null, newPassword);
        await userRepository.SaveAsync(loggedUser);
    }

    public async Task DeleteLoggedUserAccountAsync(string token)
    {
        var userId = jwtService.ExtractId(token);
        await userRepository.deleteAsync(userId);
    }

    public async Task<byte[]> Get2FAQrCodeAsync(string token)
    {
        var loggedUser = await GetLoggedUserAsync(token);
        var credentials = new GoogleAuthenticatorKey(loggedUser.secretKey2FA, loggedUser.ScratchCodes);
        return Generate2FaQrCode(loggedUser.email, credentials);
    }

    public async Task<int> Get2FAScratchCodeAsync(string token)
    {
        var loggedUser = await GetLoggedUserAsync(token);
        int scratchCode;
        try
        {
            var scratchCodes = loggedUser.scratchCodes;
            scratchCode = scratchCodes.Dequeue();
            loggedUser.scratchCodes = scratchCodes;
        }
        catch (InvalidOperationException)
        {
            var credentials = googleAuthenticator.CreateCredentials();
            loggedUser.secretKey2FA = credentials.Key;
            loggedUser.scratchCodes = credentials.scratchCodes;
            scratchCode = -1; // Indicates a new QR code is needed
        }

        await userRepository.SaveAsync(loggedUser);
        return scratchCode;
    }

    // Helper methods
    private async Task<User> GetLoggedUserAsync(string token = null)
    {
        if (token == null)
        {
            var userId = GetLoggedUserId();
            return await FindByIdAsync(userId);
        }
        else
        {
            var userId = jwtService.ExtractId(token);
            return await FindByIdAsync(userId);
        }
    }

    private async Task<User> FindByIdAsync(long id)
    {
        return await userRepository.getByIdAsync(id);
    }

    private async Task<User> FindByemailAsync(string email)
    {
        return await userRepository.getByEmailAsync(email);
    }

    private int GetTokenExpirationLength(bool stayLoggedIn)
    {
        return int.Parse(stayLoggedIn ? configuration["TokenExpirationLong"] : configuration["TokenExpirationShort"]);
    }

    private string GenerateTemporaryPassword()
    {
        using var rng = new RNGCryptoServiceProvider();
        var randomBytes = new byte[16];
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    private async Task SetDefaultSettingsAsync(long userId)
    {
        await taskUrgencyService.createDefaultItems(userId);
        await routineTimePeriodService.createDefaultItems(userId);
        await roleService.createDefaultItems(userId);
    }

    private long GetLoggedUserId()
    {
        var user = authenticationService.GetUserAsync().Result; // Simplified; consider using async method
        if (user == null || !(user is LoggedUser))
        {
            throw new UserNotInSecurityContext();
        }
        return ((LoggedUser)user).id;
    }
}


