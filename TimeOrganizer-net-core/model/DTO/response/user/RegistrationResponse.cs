using TimeOrganizer_net_core.model.DTO.response.generic;

namespace TimeOrganizer_net_core.model.DTO.response.user;

public class RegistrationResponse : EmailResponse
{
    public bool RequiresTwoFactor { get; set; }
    public byte[]? QrCode { get; set; }
    public IEnumerable<string>? ScratchCodes { get; set; }
}
