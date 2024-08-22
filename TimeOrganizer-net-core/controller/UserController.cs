using Microsoft.AspNetCore.Mvc;
using TimeOrganizer_net_core.security;
using TimeOrganizer_net_core.service;

namespace TimeOrganizer_net_core.controller;

[ApiController]
[Route("[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;
    
    
    // [HttpGet("refresh")]
    // public async Task<IActionResult> refreshToken()
    // {
    //     if (!(Request.Cookies.TryGetValue("X-Username", out var userName) && Request.Cookies.TryGetValue("X-Refresh-Token", out var refreshToken)))
    //         return BadRequest();
    //
    //     var user = _userManager.Users.FirstOrDefault(i => i.UserName == userName && i.RefreshToken == refreshToken);
    //
    //     if (user == null)
    //         return BadRequest();
    //
    //     var token = _jwtService.generateToken(user.Email, user.Id, 20);
    //
    //     user.RefreshToken = Guid.NewGuid().ToString();
    //
    //     await _userManager.UpdateAsync(user);
    //
    //     Response.Cookies.Append("X-Access-Token", token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
    //     Response.Cookies.Append("X-Username", user.UserName, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
    //     Response.Cookies.Append("X-Refresh-Token", user.RefreshToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
    //
    //     return Ok();
    // }
}