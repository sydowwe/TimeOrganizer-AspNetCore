using TimeOrganizer_net_core.model.DTO.request.extendable;

namespace TimeOrganizer_net_core.model.DTO.request.user;

public class ResetPasswordRequest : IRequest
{
    public string NewPassword { get; set; }
}