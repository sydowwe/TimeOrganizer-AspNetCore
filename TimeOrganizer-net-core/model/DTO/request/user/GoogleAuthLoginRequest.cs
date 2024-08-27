namespace TimeOrganizer_net_core.model.DTO.request.user;

public class GoogleAuthLoginRequest : EmailRequest
{
    public string Code { get; set; }
    public bool StayLoggedIn { get; set; }
    public bool RememberClient { get; set; }
}