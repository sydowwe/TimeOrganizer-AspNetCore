using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
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
        
        services.AddIdentity<User, Role>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 4;
                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
                options.Stores.ProtectPersonalData = true;
                
                options.ClaimsIdentity.UserNameClaimType = ClaimTypes.Email;
            })
            .AddEntityFrameworkStores<AppDbContext>() // Use the custom DbContext
            .AddDefaultTokenProviders();

        services.AddAuthentication()
            // .AddGoogle(options =>
            // {
            //     options.ClientId = Configuration["Authentication:Google:ClientId"];
            //     options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            // })
            .AddIdentityCookies(options =>
            {
                options.ApplicationCookie?.Configure(opt =>
                {
                    opt.Cookie.IsEssential = true;
                    opt.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    opt.Cookie.SameSite = SameSiteMode.Strict;
                    opt.Cookie.MaxAge = TimeSpan.FromHours(3);
                });
            });
        services.AddAuthorization();

        services.AddIdentityApiEndpoints<User>();
        return services;
    }
}