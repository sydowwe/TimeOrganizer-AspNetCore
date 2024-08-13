using Microsoft.EntityFrameworkCore;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository.abs;

namespace TimeOrganizer_net_core.repository;

public interface IUserRepository : IRepository<User>
{
    Task<User> getByEmailAsync(string email);
    Task<int> updatePasswordByEmailAsync(string email, string newPassword);
    Task<int> updateCurrentLocaleByIdAsync(long id, AvailableLocales currentLocale);
    Task updateStayLoggedInByIdAsync(long id, bool isStayLoggedIn);
    Task updateUserTimezoneAsync(long userId, TimeZoneInfo timezone);
}

public class UserRepository(AppDbContext context) : ParentRepository<User>(context), IUserRepository
{
    public async Task<User> getByEmailAsync(string email)
    {
        return await dbSet.FirstOrDefaultAsync(u => u.email == email);
    }

    public async Task<int> updatePasswordByEmailAsync(string email, string newPassword)
    {
        var user = await dbSet.FirstOrDefaultAsync(u => u.email == email);
        if (user != null)
        {
            user.password = newPassword;
            return await context.SaveChangesAsync();
        }
        return 0;
    }
    public async Task<int> updateCurrentLocaleByIdAsync(long id, AvailableLocales currentLocale)
    {
        var user = await dbSet.FindAsync(id);
        if (user != null)
        {
            user.currentLocale = currentLocale;
            return await context.SaveChangesAsync();
        }
        return 0;
    }
    public async Task updateStayLoggedInByIdAsync(long id, bool isStayLoggedIn)
    {
        var user = await dbSet.FindAsync(id);
        if (user != null)
        {
            user.isStayLoggedIn = isStayLoggedIn;
            await context.SaveChangesAsync();
        }
    }


    public async Task updateUserTimezoneAsync(long userId, TimeZoneInfo timezone)
    {
        var user = await dbSet.FindAsync(userId);
        if (user != null)
        {
            user.timezone = timezone;
            await context.SaveChangesAsync();
        }
    }

}