namespace TimeOrganizer_net_core.model.DTO.request.user;

public class GoogleAuthLoginRequest : EmailRequest
{
    public string code { get; set; }
}
