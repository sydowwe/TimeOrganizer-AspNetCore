using TimeOrganizer_net_core.model.entity;

namespace TimeOrganizer_net_core.security;

public class LoggedUser
{
    public long UserId { get; set; }
    public string Email { get; set; }
    public string Timezone { get; set; }
    public AvailableLocales Locale { get; set; }
    public bool? TwoFactorEnabled { get; set; }
    public IEnumerable<string> Roles { get; set; }
}
