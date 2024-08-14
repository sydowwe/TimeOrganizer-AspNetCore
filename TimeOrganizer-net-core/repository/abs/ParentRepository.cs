using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TimeOrganizer_net_core.model.DTO.response.generic;
using TimeOrganizer_net_core.model.entity.abs;

namespace TimeOrganizer_net_core.repository.abs;

public interface IRepository<T>
    where T : AbstractEntityWithUser
{
    IQueryable<T> getAsQueryable(long userId);
    Task<T?> getByIdAsync(long id);
    Task<List<T>> getAllAsync(long userId);
    Task addAsync(T entity);
    Task addRangeAsync(IEnumerable<T> entity);
    Task updateAsync(T entity);
    Task deleteAsync(long id);
    Task batchDeleteAsync(Expression<Func<T, bool>> predicate);
}

public class ParentRepository<T>(AppDbContext context) : IRepository<T>
    where T : AbstractEntityWithUser
{
    protected readonly AppDbContext context = context;
    protected readonly DbSet<T> dbSet = context.Set<T>();

    public IQueryable<T> getAsQueryable(long userId)
    {
        return dbSet.Where(e => e.userId == userId).AsQueryable();
    }
    public async Task<T?> getByIdAsync(long id)
    {
        return await dbSet.FindAsync(id);
    }
    public async Task<List<T>> getAllAsync(long userId)
    {
        return await dbSet.Where(e => e.userId == userId).ToListAsync();
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
        var entitiesToDelete = await dbSet.Where(predicate).ToListAsync();

        if (entitiesToDelete.Count != 0)
        {
            dbSet.RemoveRange(entitiesToDelete);
            await context.SaveChangesAsync();
        }
    }
}