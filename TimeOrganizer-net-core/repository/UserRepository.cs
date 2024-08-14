using Microsoft.EntityFrameworkCore;
using TimeOrganizer_net_core.exception;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository.abs;

namespace TimeOrganizer_net_core.repository;

public interface IUserRepository
{
    Task<User> getByIdAsync(long id);
    Task<User> getByEmailAsync(string email);
    Task addAsync(User user);
    Task updateAsync(User user);
    Task deleteAsync(long id);
    Task<int> updatePasswordByEmailAsync(string email, string newPassword);
    Task<int> updateCurrentLocaleByIdAsync(long id, AvailableLocales currentLocale);
    Task updateStayLoggedInByIdAsync(long id, bool isStayLoggedIn);
    Task updateUserTimezoneAsync(long userId, TimeZoneInfo timezone);
}

public class UserRepository(AppDbContext context) : IUserRepository
{
    private readonly DbSet<User> dbSet = context.Set<User>();
    
    public async Task<User> getByIdAsync(long id)
    {
        return await dbSet.FindAsync(id) ?? throw new NotFoundException($"User with ID: {id} NOT FOUND");
    }
    public async Task<User> getByEmailAsync(string email)
    {
        return await dbSet.Where(u=>u.email==email).FirstOrDefaultAsync() ?? throw new NotFoundException($"User with EMAIL: {email} NOT FOUND");
    }
    public async Task addAsync(User user)
    {
        await dbSet.AddAsync(user);
        await context.SaveChangesAsync();
    }
    public async Task updateAsync(User user)
    {
        dbSet.Update(user);
        await context.SaveChangesAsync();
    }
    public async Task deleteAsync(long id)
    {
        var entity = await dbSet.FindAsync(id);
        if (entity != null)
        {
            dbSet.Remove(entity);
            await context.SaveChangesAsync();
        }
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