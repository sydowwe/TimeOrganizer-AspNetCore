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
    #region unauthincated
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserAsync([FromBody] RegistrationRequest registrationRequest)
    {
        var result = await userService.RegisterUserAsync(registrationRequest);
        if (!result.Success)
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
        if (!result.Success)
        {
            return result.ErrorType switch
            {
                ServiceResultErrorType.BadRequest => BadRequest(result.ErrorMessage),
                ServiceResultErrorType.AuthenticationFailed => Unauthorized(result.ErrorMessage),
                ServiceResultErrorType.UserLockedOut => StatusCode(StatusCodes.Status423Locked, result.ErrorMessage),
                ServiceResultErrorType.InternalServerError => StatusCode(StatusCodes.Status500InternalServerError, result.ErrorMessage),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
            };
        }
        return Ok(result.Data);
    }
    
    [HttpPost("login-2fa")]
    public async Task<IActionResult> ValidateTwoFactorAuthLoginAsync([FromBody] TwoFactorAuthLoginRequest twoFactorAuthLoginRequest)
    {
        var result = await userService.ValidateTwoFactorAuthForLoginAsync(twoFactorAuthLoginRequest);
        if (!result.Success)
        {
            StatusCode(StatusCodes.Status500InternalServerError, result.ErrorMessage);
        }
        return Ok();
    }
  
    [AllowAnonymous]
    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string code)
    {
        var result = await userService.ConfirmEmail(email, code);
        if (!result.Success)
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
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromQuery] string email)
    {
        var result = await userService.ForgotPassword(email);
        if (!result.Success)
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
        if (!result.Success)
        {
            return result.ErrorType switch
            {
                ServiceResultErrorType.IdentityError => StatusCode(StatusCodes.Status500InternalServerError, "Error with UserManager"),
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
        await userService.Logout(HttpContext);
        return Ok();
    }
    [HttpPut("change-locale/{locale}")]
    public async Task<IActionResult> ChangeCurrentLocaleAsync([FromRoute] AvailableLocales locale)
    {
        var result = await userService.ChangeCurrentLocaleAsync(locale);
        if (!result.Success)
        {
            return result.ErrorType switch
            {
                ServiceResultErrorType.IdentityError => StatusCode(StatusCodes.Status500InternalServerError, "Error with UserManager"),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
            };
        }
        return Ok();
    }


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
    [HttpPost("verify")]
    public async Task<IActionResult> VerifyUserAsync([FromBody] VerifyUserRequest request)
    {
        var result = await userService.VerifyUserAsync(request);
        if (!result.Success)
        {
            return result.ErrorType switch
            {
                _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
            };
        }
        return Ok();
    }

    [HttpPut("edit")]
    public async Task<IActionResult> EditUserDataAsync([FromBody] UserRequest request)
    {
        var result = await userService.EditUserDataAsync(request);
        return Ok(result);
    }
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequest request)
    {
        var result = await userService.ChangePasswordAsync(request);
        if (!result.Success)
        {
            return result.ErrorType switch
            {
                ServiceResultErrorType.IdentityError => StatusCode(StatusCodes.Status500InternalServerError, "Error with UserManager"),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
            };
        }
        return Ok();
    }
    [HttpPost("delete-account")]
    public async Task<IActionResult> DeleteUserAccountAsync()
    {
        var result = await userService.DeleteUserAccountAsync();
        if (!result.Success)
        {
            return result.ErrorType switch
            {
                ServiceResultErrorType.IdentityError => StatusCode(StatusCodes.Status500InternalServerError, "Error with UserManager"),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
            };
        }
        return Ok();
    }

    [HttpPost("2fa-qr-code")]
    public async Task<IActionResult> GetTwoFactorAuthQrCodeAsync()
    {
        var qrCode = await userService.GetTwoFactorAuthQrCodeAsync();
        if (qrCode == null)
        {
            return BadRequest("Failed to generate QR code");
        }
        return File(qrCode, "image/png");
    }

    [HttpPost("2fa-scratch-codes")]
    public async Task<IActionResult> GenerateTwoFactorAuthScratchCodesAsync()
    {
        var scratchCodes = await userService.GenerateTwoFactorAuthScratchCodesAsync();
        if (scratchCodes == null)
        {
            return BadRequest("Failed to generate scratch codes");
        }
        return Ok(scratchCodes);
    }
}