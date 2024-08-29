namespace TimeOrganizer_net_core.model.DTO.response.user;

public class EditedUserResponse : UserResponse
{
    public IEnumerable<string>? ScratchCodes { get; set; }
    public byte[]? QrCode { get; set; }

    public EditedUserResponse(UserResponse userResponse)
    {
        Name = userResponse.Name;
        Surname = userResponse.Surname;
        Email = userResponse.Email;
        TwoFactorEnabled = userResponse.TwoFactorEnabled;
    }
}
