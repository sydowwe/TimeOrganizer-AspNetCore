using TimeOrganizer_net_core.model.DTO.request.extendable;

namespace TimeOrganizer_net_core.model.DTO.request.user;

public class ChangePasswordRequest : IRequest
{
    public string? TwoFactorAuthToken { get; set; }
    public string CurrentPassword {get;set;}
    public string NewPassword {get;set;}
}