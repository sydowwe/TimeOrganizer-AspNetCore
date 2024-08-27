namespace TimeOrganizer_net_core.model.DTO.request.user;

public class UserRequest : EmailRequest
{
    public string name { get; set; }
    public string surname { get; set; }
    public bool TwoFactorEnabled { get; set; }
}
