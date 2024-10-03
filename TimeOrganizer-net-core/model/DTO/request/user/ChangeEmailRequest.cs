namespace TimeOrganizer_net_core.model.DTO.request.user;

public class ChangeEmailRequest : VerifyUserRequest
{
    public string NewEmail { get; set; }
}