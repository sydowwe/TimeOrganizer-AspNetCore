using TimeOrganizer_net_core.model.DTO.response.generic;
using TimeOrganizer_net_core.model.entity;

namespace TimeOrganizer_net_core.model.DTO.response.user;

public class LoginResponse : IResponse
{
    public bool RequiresTwoFactor { get; set; }
    public AvailableLocales CurrentLocale { get; set; }
}
