using TimeOrganizer_net_core.model.DTO.response.generic;

namespace TimeOrganizer_net_core.model.DTO.response.user;

public class RegistrationResponse : IResponse
{
    public bool TwoFactorEnabled { get; set; }
    public byte[]? QrCode { get; set; }
    public IEnumerable<string>? RecoveryCodes { get; set; }
}
