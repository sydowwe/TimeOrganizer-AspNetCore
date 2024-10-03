namespace TimeOrganizer_net_core.model.DTO.request.user;

public class UserRequest : EmailRequest
{
    public bool TwoFactorEnabled { get; set; }
}
