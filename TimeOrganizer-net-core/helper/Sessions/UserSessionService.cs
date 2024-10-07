namespace TimeOrganizer_net_core.helper.Sessions;

public interface IUserSessionService
{
    void SetForPasswordReset(string userId, string resetToken);
    (string UserId, string Token) GetPasswordResetSession();
    void ClearPasswordResetSession();
}

public class UserSessionService(IHttpContextAccessor httpContextAccessor) : IUserSessionService
{
    private readonly dynamic _session = httpContextAccessor.HttpContext?.Session ?? throw new Exception("No Active HttpContext");
    public void SetForPasswordReset(string userId, string resetToken)
    {
        _session.SetString("UserId", userId);
        _session.SetString("ResetToken", resetToken);
    }

    public (string UserId, string Token) GetPasswordResetSession()
    {
        return (_session.GetString("UserId"), _session.GetString("ResetToken"));
    }

    public void ClearPasswordResetSession()
    {
        _session.Remove("UserId");
        _session.Remove("ResetToken");
    }
}
