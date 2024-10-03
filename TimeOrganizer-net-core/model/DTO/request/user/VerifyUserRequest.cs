using TimeOrganizer_net_core.model.DTO.request.extendable;

namespace TimeOrganizer_net_core.model.DTO.request.user;

public class VerifyUserRequest : IRequest
{
    public string? TwoFactorAuthToken { get; set; }
    public string Password {get;set;}
}