namespace TimeOrganizer_net_core.model.DTO.response.user;

public class EditedUserResponse : UserResponse
{
    public IEnumerable<string>? scratchCodes { get; set; }
    public byte[]? qrCode { get; set; }

    public EditedUserResponse(UserResponse userResponse)
    {
        name = userResponse.name;
        surname = userResponse.surname;
        email = userResponse.email;
        twoFactorEnabled = userResponse.twoFactorEnabled;
    }
}
