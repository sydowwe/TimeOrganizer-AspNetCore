using System.Security.Authentication;
using System.Security.Cryptography;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
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
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    IHttpContextAccessor httpContextAccessor,
    IGoogleRecaptchaService googleRecaptchaService,
    ITOTPService totpService,
    // IEmailService emailService,
    ITaskUrgencyService taskUrgencyService,
    IRoutineTimePeriodService routineTimePeriodService,
    IRoleService roleService,
    IMapper mapper,
    IConfiguration configuration) : IUserService
{
    public async Task<RegistrationResponse> registerUserAsync(RegistrationRequest registration)
    {
        if (!await googleRecaptchaService.verifyRecaptchaAsync(registration.recaptchaToken, "register"))
        {
            throw new ReCaptchaException("Wrong captcha token or action");
        }

        var newUser = new User
        {
            name = registration.name,
            surname = registration.surname,
            Email = registration.email,
            UserName = registration.email,
            currentLocale = registration.currentLocale,
            timezone = registration.timezone,
            isStayLoggedIn = false
        };
        var result = await userManager.CreateAsync(newUser, registration.password);
        if (!result.Succeeded)
        {
            throw new Exception("Failed to register user " + result.Errors);
        }

        RegistrationResponse response;
        if (registration.has2FA)
        {
            newUser.secretKey2FA = totpService.generateSecretKey();
            var qrCode = totpService.generateQrCode(newUser.secretKey2FA, newUser.Email);
            response = new RegistrationResponse
            {
                email = newUser.Email,
                has2FA = true,
                qrCode = qrCode
            };
        }
        else
        {
            response = new RegistrationResponse
            {
                email = newUser.Email,
                has2FA = false
            };
        }

        await setDefaultSettingsAsync(newUser.Id);
        return response;
    }

    public async Task<LoginResponse> loginUserAsync(LoginRequest loginRequest)
    {
        if (!await googleRecaptchaService.verifyRecaptchaAsync(loginRequest.recaptchaToken, "login"))
        {
            throw new ReCaptchaException("Wrong captcha token or action");
        }


        var user = await userManager.FindByEmailAsync(loginRequest.email);
        if (user == null)
        {
            throw new UserNotFoundException(loginRequest.email);
        }

        var result = await signInManager.PasswordSignInAsync(loginRequest.email, loginRequest.password, loginRequest.stayLoggedIn, true);

        if (result.IsNotAllowed)
        {
            await userManager.AccessFailedAsync(user);
            throw new AuthenticationException();
        }
        if (result.IsLockedOut)
        {
            throw new UserLockedOutException(5);
        }
        if (!result.Succeeded)
        {
            throw new Exception(result.ToString());
        }
        if (result.RequiresTwoFactor)
        {
            //TODO
        }

        if (user.isStayLoggedIn != loginRequest.stayLoggedIn)
        {
            user.isStayLoggedIn = loginRequest.stayLoggedIn;
        }

        if (!user.timezone.Equals(loginRequest.timezone))
        {
            user.timezone = loginRequest.timezone;
        }

        await userManager.UpdateAsync(user);
        return new LoginResponse
        {
            id = user.Id,
            email = loginRequest.email,
            has2FA = user.TwoFactorEnabled,
            currentLocale = user.currentLocale
        };
    }

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


    public async Task<GoogleAuthResponse> validate2FALoginAsync(GoogleAuthLoginRequest request)
    {
        var user = await getByEmailAsync(request.email);
        if (user == null)
        {
            throw new NotFoundException($"User with email: {request.email} not found");
        }

        var is2FAValid = totpService.validateTotp(user.secretKey2FA, request.code);
        var response = new GoogleAuthResponse
        {
            authorized = is2FAValid,
            token = is2FAValid ? jwtService.generateToken(user.email, user.id, getTokenExpirationLength(user.isStayLoggedIn)) : null
        };

        if (is2FAValid)
        {
        }
        else
        {
        }

        return response;
    }

    public void logout(HttpContext context)
    {
        httpContextAccessor.HttpContext?.Session.Clear();
    }

    public async Task changeCurrentLocaleAsync(AvailableLocales locale)
    {
        var loggedUserId = getLoggedUserId();
        await userRepository.updateCurrentLocaleByIdAsync(loggedUserId, locale);
    }

    public async Task resetPasswordAsync(string email)
    {
        var tempPassword = generateTemporaryPassword();
        var updated = await userRepository.updatePasswordByEmailAsync(email, passwordHasher.HashPassword(null, tempPassword));
        if (updated < 1)
        {
            throw new InvalidOperationException();
        }

        var emailBody = emailService.GenerateForgottenPasswordemail(tempPassword);
        await emailService.SendemailAsync(email, $"Password reset - {configuration["App:Name"]}", emailBody);
    }

    public async Task<bool> wereSensitiveChangesMadeAsync(UserRequest changedUser)
    {
        var loggedUser = await GetLoggedUserAsync();
        return !(loggedUser.email.Equals(changedUser.email) && loggedUser.has2FA == changedUser.has2FA);
    }

    public async Task<bool> verifyPasswordAndReturn2FAStatusAsync(string token, string password)
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

    public async Task<bool> validate2FAAsync(string token, int code)
    {
        var loggedUser = await GetLoggedUserAsync(token);
        return googleAuthenticator.Authorize(loggedUser.secretKey2FA, code);
    }

    public async Task<UserResponse> getLoggedUserDataAsync()
    {
        var loggedUser = await GetLoggedUserAsync();
        return mapper.Map<UserResponse>(loggedUser);
    }

    public async Task<EditedUserResponse> editLoggedUserDataAsync(string token, UserRequest request)
    {
        var loggedUser = await GetLoggedUserAsync(token);
        string newToken;
        byte[] qrCode;

        if (!loggedUser.email.Equals(request.email))
        {
            jwtService.invalidateToken(token);
            newToken = jwtService.generateToken(request.email, loggedUser.id, getTokenExpirationLength(loggedUser.isStayLoggedIn));
        }

        if (!loggedUser.has2FA() && request.has2FA)
        {
            loggedUser.secretKey2FA = totpService.generateSecretKey();
            loggedUser.scratchCodes = null;
            qrCode = totpService.generateQrCode(loggedUser.secretKey2FA, loggedUser.email);
        }

        mapper.Map(loggedUser, request);
        await userRepository.updateAsync(loggedUser);

        return userMapper.ConvertToEditedUserSettingsResponse(loggedUser, newToken, qrCode);
    }

    public async Task changeLoggedUserPasswordAsync(string token, string newPassword)
    {
        var loggedUser = await GetLoggedUserAsync(token);
        loggedUser.passwordHash = passwordHasher.HashPassword(null, newPassword);
        await userRepository.SaveAsync(loggedUser);
    }

    public async Task deleteLoggedUserAccountAsync(string token)
    {
        var userId = jwtService.extractId(token);
        await userRepository.deleteAsync(userId);
    }

    public async Task<byte[]> get2FAQrCodeAsync()
    {
        var loggedUser = await getLoggedUserAsync();
        return totpService.generateQrCode(loggedUser.email, loggedUser.secretKey2FA);
    }

    // public async Task<int> Get2FAScratchCodeAsync(string token)
    // {
    //     var loggedUser = await GetLoggedUserAsync(token);
    //     int scratchCode;
    //     try
    //     {
    //         var scratchCodes = loggedUser.scratchCodes;
    //         scratchCode = scratchCodes.Dequeue();
    //         loggedUser.scratchCodes = scratchCodes;
    //     }
    //     catch (InvalidOperationException)
    //     {
    //         loggedUser.secretKey2FA = totpService.generateSecretKey();
    //     }
    //
    //     await userRepository.SaveAsync(loggedUser);
    //     return scratchCode;
    // }


    private string generateTemporaryPassword()
    {
        using var rng = RandomNumberGenerator.Create();
        var randomBytes = new byte[16];
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    private long getLoggedUserId()
    {
        var user = authenticationService.GetUserAsync().Result; // Simplified; consider using async method
        if (user == null || !(user is LoggedUser))
        {
            throw new UserNotInSecurityContext();
        }

        return ((LoggedUser)user).id;
    }

    private async Task<User> getLoggedUserAsync()
    {
        return new User();
    }

    private async Task setDefaultSettingsAsync(long userId)
    {
        await taskUrgencyService.createDefaultItems(userId);
        await routineTimePeriodService.createDefaultItems(userId);
        await roleService.createDefaultItems(userId);
    }

    private async Task<User> getByIdAsync(long id)
    {
        return await userRepository.getByIdAsync(id);
    }

    private async Task<User> getByEmailAsync(string email)
    {
        return await userRepository.getByEmailAsync(email);
    }

    private int getTokenExpirationLength(bool stayLoggedIn)
    {
        return int.Parse(
            (stayLoggedIn
                ? Environment.GetEnvironmentVariable("TOKEN_EXPIRATION_LONG")
                : Environment.GetEnvironmentVariable("TOKEN_EXPIRATION_SHORT")) ?? throw new Exception("No env variables for token length"));
    }
}