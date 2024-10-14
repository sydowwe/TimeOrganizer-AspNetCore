using TimeOrganizer_net_core.model.DTO.request.extendable;

namespace TimeOrganizer_net_core.model.DTO.request.user;

public class ResetPasswordRequest : IRequest
{
    public long UserId { get; set; }
    public string NewPassword { get; set; }
    public string Token { get; set; }
}