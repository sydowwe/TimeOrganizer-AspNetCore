namespace TimeOrganizer_net_core.model.DTO.response.user;

public class EditedUserResponse : UserResponse
{
    public string token { get; set; }
    public byte[] qrCode { get; set; }
}
