using TimeOrganizer_net_core.model.DTO.response.generic;

namespace TimeOrganizer_net_core.model.DTO.response.user;

public class TwoFactorAuthResponse : IResponse
{
    public bool TwoFactorEnabled { get; set; }
    public string? QrCode { get; set; }
    public IEnumerable<string>? RecoveryCodes { get; set; }
}
