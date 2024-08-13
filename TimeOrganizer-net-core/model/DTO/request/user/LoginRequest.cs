namespace TimeOrganizer_net_core.model.DTO.request.user;

public class LoginRequest : EmailRequest
{
    public string password { get; set; }
    public bool stayLoggedIn { get; set; }
    public string recaptchaToken { get; set; }
    public TimeZoneInfo timezone { get; set; } 
}