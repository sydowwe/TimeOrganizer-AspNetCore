using System.Security.Claims;

namespace TimeOrganizer_net_core.security;

public interface ILoggedUserService
{
    ClaimsPrincipal GetLoggedUserPrincipal();
    LoggedUser GetLoggedUser();
    long GetLoggedUserId();
    bool IsAuthenticated();
}

public class LoggedUserService(IHttpContextAccessor httpContextAccessor) : ILoggedUserService
{
    public ClaimsPrincipal GetLoggedUserPrincipal()
    {
        var user = httpContextAccessor.HttpContext?.User;
        //TODO create exceptions
        if (user == null)
        {
            throw new Exception("user not in context");
        }
        if (user.Identity is not { IsAuthenticated: true })
        {
            throw new Exception("user not authenticated");
        }
        return user;
    }
    public LoggedUser GetLoggedUser()
    {
        var user = GetLoggedUserPrincipal();
        return new LoggedUser
        {
            UserId = long.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("Id missing in claims")),
            Email = user.FindFirst(ClaimTypes.Email)?.Value ?? throw new InvalidOperationException("Missing email in claims"),
            Roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList(),
        };
    }

    public long GetLoggedUserId()
    {
        return GetLoggedUser().UserId;
    }

    public bool IsAuthenticated()
    {
        try
        {
            GetLoggedUserPrincipal();
        }
        catch (Exception e)
        {
            return false;
        }
        return true;
    }
}