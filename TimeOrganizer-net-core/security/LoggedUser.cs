namespace TimeOrganizer_net_core.security;

public class LoggedUser
{
    public long UserId { get; set; }
    public string Email { get; set; }
    public TimeZoneInfo Timezone { get; set; }
    public string Locale { get; set; }
    public IEnumerable<string> Roles { get; set; }
}
