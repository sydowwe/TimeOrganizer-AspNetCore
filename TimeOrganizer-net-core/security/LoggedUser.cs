using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using TimeOrganizer_net_core.model.entity;

namespace TimeOrganizer_net_core.security;

public class LoggedUser : IdentityUser
{
    public long id { get; set; }
    public UserRole role { get; set; }
    public bool isStayLoggedIn { get; set; }
    public bool has2FA { get; set; }
    public string name { get; set; }
    public string surname { get; set; }
    public AvailableLocales currentLocale { get; set; }
    public TimeZoneInfo timezone { get; set; }

    public IList<Claim> getClaims()
    {
        return new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, id.ToString()),
            new Claim(ClaimTypes.Email, Email),
            new Claim(ClaimTypes.Role, role.ToString()),
            new Claim("Locale", currentLocale.ToString()),
            new Claim("Timezone", timezone.Id)
        };
    }
}