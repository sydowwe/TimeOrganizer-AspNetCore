using TimeOrganizer_net_core.model.DTO.request.extendable;

namespace TimeOrganizer_net_core.model.DTO.request.user;

public class TwoFactorAuthRequest : IRequest
{
    public string Token { get; set; }
}