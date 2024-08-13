using System.Security.Claims;
using TimeOrganizer_net_core.repository;
using TimeOrganizer_net_core.security;

namespace TimeOrganizer_net_core.service;

public interface IUserService
{
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // public async Task<LoggedUser> getLoggedUserAsync()
    // {
    //     var user = _httpContextAccessor.HttpContext.User;
    //
    //     if (user == null || !user.Identity.IsAuthenticated)
    //     {
    //         throw new UnauthorizedAccessException("User is not authenticated");
    //     }
    //
    //     var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //
    //     if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
    //     {
    //         throw new UnauthorizedAccessException("Invalid user ID claim");
    //     }
    //
    //     var dbUser = await _userRepository.GetByIdAsync(userId);
    //
    //     if (dbUser == null)
    //     {
    //         throw new UnauthorizedAccessException("User not found in database");
    //     }
    // }
}

