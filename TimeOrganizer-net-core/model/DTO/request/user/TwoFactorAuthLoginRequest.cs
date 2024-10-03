using TimeOrganizer_net_core.model.DTO.request.extendable;

namespace TimeOrganizer_net_core.model.DTO.request.user;

public class TwoFactorAuthLoginRequest : TwoFactorAuthRequest
{
    public bool StayLoggedIn { get; set; }
    // public bool RememberClient { get; set; }
}