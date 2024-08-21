using Microsoft.AspNetCore.Identity;
using TimeOrganizer_net_core.model.entity;

namespace TimeOrganizer_net_core.security;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.Configure<IdentityOptions>(options =>
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
        });
        services.AddIdentity<
                User, IdentityRole<long>>()
            .AddUserStore<User>()
            .AddDefaultTokenProviders();

        return services;
    }
}