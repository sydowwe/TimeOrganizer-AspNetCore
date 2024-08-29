using TimeOrganizer_net_core.model.entity;

namespace TimeOrganizer_net_core.model.DTO.request.user;

public class RegistrationRequest : UserRequest
{
    public string Password { get; set; }
    public string RecaptchaToken { get; set; }
    public AvailableLocales CurrentLocale { get; set; }
    public string Timezone { get; set; }
}
