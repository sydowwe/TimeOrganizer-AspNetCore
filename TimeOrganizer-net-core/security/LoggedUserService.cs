using System.Security.Claims;
using TimeOrganizer_net_core.exception;

namespace TimeOrganizer_net_core.security;

public interface ILoggedUserService
{
    ClaimsPrincipal GetLoggedUserPrincipal();
    LoggedUser GetLoggedUser();
    long GetLoggedUserId();
    TimeZoneInfo GetLoggedUserTimezone();
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
            UserId = long.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new ClaimMissingException("ID")),
            Email = user.FindFirst(ClaimTypes.Email)?.Value ?? throw new ClaimMissingException("EMAIL"),
            Roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList(),
            Timezone = TimeZoneInfo.FindSystemTimeZoneById(user?.FindFirst("timezone")?.Value ?? throw new ClaimMissingException("TIMEZONE"))
        };
    }

    public long GetLoggedUserId()
    {
        return GetLoggedUser().UserId;
    }
    public TimeZoneInfo GetLoggedUserTimezone()
    {
        return GetLoggedUser().Timezone;
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