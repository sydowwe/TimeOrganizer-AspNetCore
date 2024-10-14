namespace TimeOrganizer_net_core.helper.Sessions;

public interface IUserSessionService
{
}

public class UserSessionService(IHttpContextAccessor httpContextAccessor) : IUserSessionService
{
    private readonly dynamic _session = httpContextAccessor.HttpContext?.Session ?? throw new Exception("No Active HttpContext");
 
}
