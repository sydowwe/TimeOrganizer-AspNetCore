using TimeOrganizer_net_core.model.DTO.response.generic;

namespace TimeOrganizer_net_core.model.DTO.response.user;

public class UserResponse : IdResponse
{
    public string name { get; set; }
    public string surname { get; set; }
    public string email { get; set; }
    public bool has2FA { get; set; }
}
