using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using TimeOrganizer_net_core.helper;
using TimeOrganizer_net_core.model.entity;

namespace TimeOrganizer_net_core.security;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        // services.ConfigureApplicationCookie(options =>
        // {
        //     // Cookie settings
        //     options.Cookie.HttpOnly = true; // Prevents client-side scripts from accessing the cookie
        //     options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Cookie expiration time
        //
        //     options.LoginPath = "/Account/Login"; // Redirect to login page
        //     options.AccessDeniedPath = "/Account/AccessDenied"; // Redirect to access denied page
        //     options.SlidingExpiration = true; // Resets the expiration time if the user is active
        // });
        
        services.AddIdentity<User, UserRole>(options =>
            {
                // Password settings.
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 4;
                // Lockout settings.

                options.User.RequireUniqueEmail = true;
                // options.Stores.ProtectPersonalData = true;
                options.ClaimsIdentity.UserNameClaimType = ClaimTypes.Email;
            })
            .AddEntityFrameworkStores<AppDbContext>() // Use the custom DbContext
            .AddDefaultTokenProviders()
            // .AddPersonalDataProtection<>()
            .AddApiEndpoints();
        // services.AddAuthentication()
        //     .AddGoogle(options =>
        //     {
        //         options.ClientId = Helper.getEnvVar("OAUTH2_GOOGLE_CLIENT_ID");
        //         options.ClientSecret = Helper.getEnvVar("OAUTH2_GOOGLE_CLIENT_SECRET");
        //         options.CallbackPath = Helper.getEnvVar("OAUTH2_GOOGLE_REDIRECT_URI");
        //     });
            // .AddIdentityCookies(options =>
            // {
            //     options.ApplicationCookie?.Configure(opt =>
            //     {
            //         opt.Cookie.IsEssential = true;
            //         opt.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            //         opt.Cookie.SameSite = SameSiteMode.Strict;
            //         opt.Cookie.MaxAge = TimeSpan.FromHours(3);
            //     });
            // });
        services.AddAuthorization();

        return services;
    }
}