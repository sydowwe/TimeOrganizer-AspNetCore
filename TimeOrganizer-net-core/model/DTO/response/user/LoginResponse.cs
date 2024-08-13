using TimeOrganizer_net_core.model.DTO.response.generic;
using TimeOrganizer_net_core.model.entity;

namespace TimeOrganizer_net_core.model.DTO.response.user;

public class LoginResponse : IdResponse
{
    public string email { get; set; }
    public string token { get; set; }
    public bool has2FA { get; set; }
    public AvailableLocales currentLocale { get; set; }
}
