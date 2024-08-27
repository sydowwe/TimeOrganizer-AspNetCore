using System.Security.Claims;

namespace TimeOrganizer_net_core.exception;

public class UserByPrincipalNotFoundException : NotFoundException
{
    public UserByPrincipalNotFoundException(ClaimsPrincipal principal) : base($"User with ID: {principal.Identity?.Name} was not found")
    {
        
    }
}