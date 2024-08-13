namespace TimeOrganizer_net_core.model.DTO.response.user;

public class RegistrationResponse
{
    public string email { get; set; }
    public bool has2FA { get; set; }
    public byte[] qrCode { get; set; }
}
