using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TimeOrganizer_net_core.model.entity.abs;

namespace TimeOrganizer_net_core.repository.abs;

public interface IRepository<T> where T : class
{
    Task<T?> getByIdAsync(long id);
    Task<List<T>?> getAllAsync();
    Task addAsync(T entity);
    Task addRangeAsync(IEnumerable<T> entity);
    Task updateAsync(T entity);
    Task deleteAsync(long id);
    Task batchDeleteAsync(Expression<Func<T, bool>> predicate);
}
public class ParentRepository<T>(AppDbContext context) : IRepository<T>
    where T : AbstractEntity
{
    protected readonly AppDbContext context = context;
    protected readonly DbSet<T> dbSet = context.Set<T>();

    public async Task<T?> getByIdAsync(long id)
    {
        return await dbSet.FindAsync(id);
    }

    public async Task<List<T>> getAllAsync()
    {
        return await dbSet.ToListAsync();
    }

    public async Task addAsync(T entity)
    {
        await dbSet.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task addRangeAsync(IEnumerable<T> entity)
    {
        await dbSet.AddRangeAsync(entity);
        await context.SaveChangesAsync();
    }
    public async Task updateAsync(T entity)
    {
        dbSet.Update(entity);
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
    public async Task batchDeleteAsync(Expression<Func<T, bool>> predicate)
    {
        // Fetch the entities to delete
        var entitiesToDelete = await dbSet.Where(predicate).ToListAsync();
        
        if (entitiesToDelete.Count != 0)
        {
            dbSet.RemoveRange(entitiesToDelete);
            await context.SaveChangesAsync();
        }
    }
}
