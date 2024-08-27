namespace TimeOrganizer_net_core.model.DTO.request.user;

public class ResetPasswordRequest : EmailRequest
{
    public string ResetCode { get; set; }
    public string NewPassword { get; set; }
}