using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeOrganizer_net_core.helper;
using TimeOrganizer_net_core.model.DTO.request.user;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.service;

namespace TimeOrganizer_net_core.controller;

[ApiController]
[Route("[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    #region NotAuthenicated

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserAsync([FromBody] RegistrationRequest registrationRequest)
    {
        var result = await userService.RegisterUserAsync(registrationRequest);
        if (!result.Succeeded)
        {
            return result.ErrorType switch
            {
                ServiceResultErrorType.BadRequest => BadRequest(result.ErrorMessage),
                ServiceResultErrorType.Conflict => Conflict(result.ErrorMessage),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
            };
        }

        return Ok(result.Data);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginUserAsync([FromBody] LoginRequest loginRequest)
    {
        var result = await userService.LoginUserAsync(loginRequest);
        if (!result.Succeeded)
        {
            return result.ErrorType switch
            {
                ServiceResultErrorType.BadRequest => BadRequest(result.ErrorMessage),
                ServiceResultErrorType.AuthenticationFailed => Unauthorized(result.ErrorMessage),
                ServiceResultErrorType.UserLockedOut => StatusCode(StatusCodes.Status423Locked, result.ErrorMessage),
                ServiceResultErrorType.InternalServerError => StatusCode(StatusCodes.Status500InternalServerError,
                    result.ErrorMessage),
                ServiceResultErrorType.NotFound => StatusCode(StatusCodes.Status404NotFound, result.ErrorMessage),
                ServiceResultErrorType.EmailNotConfirmed => StatusCode(StatusCodes.Status412PreconditionFailed, result.ErrorMessage),
                ServiceResultErrorType.TwoFactorAuthRequired => StatusCode(StatusCodes.Status401Unauthorized,
                    result.ErrorMessage),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
            };
        }

        return Ok(result.Data);
    }

    [AllowAnonymous]
    [HttpPost("login-2fa")]
    public async Task<IActionResult> ValidateTwoFactorAuthLoginAsync(
        [FromBody] TwoFactorAuthLoginRequest twoFactorAuthLoginRequest)
    {
        var result = await userService.ValidateTwoFactorAuthForLoginAsync(twoFactorAuthLoginRequest);
        if (!result.Succeeded)
        {
            StatusCode(StatusCodes.Status500InternalServerError, result.ErrorMessage);
        }

        return Ok();
    }

    [AllowAnonymous]
    [HttpGet("resend-confirmation-email")]
    public async Task<IActionResult> ResendConfirmationEmail([FromQuery] long userId)
    {
        var result = await userService.ResendConfirmationEmail(userId);
        if (!result.Succeeded)
        {
            return result.ErrorType switch
            {
                ServiceResultErrorType.BadRequest => BadRequest(result.ErrorMessage),
                ServiceResultErrorType.NotFound => NotFound(result.ErrorMessage),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
            };
        }
        return Ok();
    }
    
    [AllowAnonymous]
    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] long userId, [FromQuery] string token)
    {
        var result = await userService.ConfirmEmail(userId, token);
        if (!result.Succeeded)
        {
            return result.ErrorType switch
            {
                ServiceResultErrorType.BadRequest => BadRequest(result.ErrorMessage),
                ServiceResultErrorType.NotFound => NotFound(result.ErrorMessage),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
            };
        }

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("forgotten-password")]
    public async Task<IActionResult> ForgottenPassword([FromBody] EmailRequest request)
    {
        var result = await userService.ForgottenPassword(request.Email);
        if (!result.Succeeded)
        {
            return result.ErrorType switch
            {
                ServiceResultErrorType.EmailNotConfirmed => Forbid(result.ErrorMessage!),
                ServiceResultErrorType.NotFound => NotFound(result.ErrorMessage),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
            };
        }

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPasswordRequest)
    {
        var result = await userService.ResetPassword(resetPasswordRequest);
        if (!result.Succeeded)
        {
            return result.ErrorType switch
            {
                ServiceResultErrorType.IdentityError => StatusCode(StatusCodes.Status500InternalServerError,
                    "Error with UserManager"),
                ServiceResultErrorType.NotFound => NotFound(result.ErrorMessage),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
            };
        }

        return Ok();
    }

    #endregion

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await userService.Logout();
        return Ok();
    }

    [HttpPut("change-locale/{locale}")]
    public async Task<IActionResult> ChangeCurrentLocaleAsync([FromRoute] AvailableLocales locale)
    {
        var result = await userService.ChangeCurrentLocaleAsync(locale);
        if (!result.Succeeded)
        {
            return result.ErrorType switch
            {
                ServiceResultErrorType.IdentityError => StatusCode(StatusCodes.Status500InternalServerError,
                    "Error with UserManager"),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
            };
        }

        return Ok();
    }

    #region UserSettings

    [HttpPost("data")]
    public async Task<IActionResult> GetLoggedUserDataAsync()
    {
        var userResponse = await userService.GetLoggedUserDataAsync();
        return Ok(userResponse);
    }

    [HttpPost("get-2fa-status")]
    public async Task<IActionResult> GetTwoFactorAuthStatusAsync()
    {
        return Ok(await userService.GetTwoFactorAuthStatusAsync());
    }

    [HttpPost("toggle-two-factor-auth")]
    public async Task<IActionResult> ToggleTwoFactorAuth([FromBody] VerifyUserRequest request)
    {
        return Ok(await userService.ToggleTwoFactorAuthAsync(request));
    }

    [HttpPost("change-email")]
    public async Task<IActionResult> ChangeEmailAsync([FromBody] ChangeEmailRequest request)
    {
        var result = await userService.ChangeEmailAsync(request);
        if (!result.Succeeded)
        {
            return result.ErrorType switch
            {
                ServiceResultErrorType.IdentityError => StatusCode(StatusCodes.Status500InternalServerError,
                    "Error with UserManager"),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
            };
        }

        return Ok();
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequest request)
    {
        var result = await userService.ChangePasswordAsync(request);
        if (!result.Succeeded)
        {
            return result.ErrorType switch
            {
                ServiceResultErrorType.IdentityError => StatusCode(StatusCodes.Status500InternalServerError,
                    "Error with UserManager"),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
            };
        }

        return Ok();
    }

    //TODO Dokoncit error types
    [HttpPost("delete-account")]
    public async Task<IActionResult> DeleteUserAccountAsync([FromBody] VerifyUserRequest request)
    {
        var result = await userService.DeleteUserAccountAsync(request);
        if (!result.Succeeded)
        {
            return result.ErrorType switch
            {
                ServiceResultErrorType.IdentityError => StatusCode(StatusCodes.Status500InternalServerError,
                    "Error with UserManager"),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
            };
        }

        return Ok();
    }

    [HttpPost("new-2fa-qr-code")]
    public async Task<IActionResult> GetTwoFactorAuthQrCodeAsync([FromBody] VerifyUserRequest request)
    {
        var result = await userService.GenerateNewTwoFactorAuthQrCodeAsync(request);
        if (!result.Succeeded)
        {
            return result.ErrorType switch
            {
                ServiceResultErrorType.IdentityError => StatusCode(StatusCodes.Status500InternalServerError,
                    "Error when generating new secret: " + result.ErrorMessage),
                ServiceResultErrorType.NotFound => StatusCode(StatusCodes.Status404NotFound,
                    "Error when getting new secret: " + result.ErrorMessage),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
            };
        }
        return Ok(result.Data);
    }

    [HttpPost("new-2fa-recovery-codes")]
    public async Task<IActionResult> GenerateNewTwoFactorAuthRecoveryCodesAsync([FromBody] VerifyUserRequest request)
    {
        var result = await userService.GenerateNewTwoFactorAuthRecoveryCodesAsync(request);
        if (!result.Succeeded)
        {
            return result.ErrorType switch
            {
                ServiceResultErrorType.IdentityError => StatusCode(StatusCodes.Status500InternalServerError,
                    result.ErrorMessage),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
            };
        }

        return Ok(result.Data);
    }

    #endregion
}