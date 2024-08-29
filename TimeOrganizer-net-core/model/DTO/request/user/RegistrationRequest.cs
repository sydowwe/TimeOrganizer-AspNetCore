using TimeOrganizer_net_core.model.entity;

namespace TimeOrganizer_net_core.model.DTO.request.user;

public class RegistrationRequest : UserRequest
{
    public string password { get; set; }
    public string recaptchaToken { get; set; }
    public AvailableLocales currentLocale { get; set; }
    public string Timezone { get; set; }
}
