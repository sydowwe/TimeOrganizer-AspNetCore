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
    IQueryable<T> GetAsQueryable(long userId);
    Task<T?> GetByIdAsync(long id);
    Task<List<T>> GetAllAsync(long userId);
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(long id);
    Task BatchDeleteAsync(Expression<Func<T, bool>> predicate);
}

public class ParentRepository<T>(AppDbContext context) : IRepository<T>
    where T : AbstractEntityWithUser
{
    protected readonly AppDbContext context = context;
    protected readonly DbSet<T> dbSet = context.Set<T>();

    public IQueryable<T> GetAsQueryable(long userId)
    {
        return dbSet.Where(e => e.UserId == userId).AsQueryable();
    }
    public async Task<T?> GetByIdAsync(long id)
    {
        return await dbSet.FindAsync(id);
    }
    public async Task<List<T>> GetAllAsync(long userId)
    {
        return await dbSet.Where(e => e.UserId == userId).ToListAsync();
    }
    public async Task AddAsync(T entity)
    {
        await dbSet.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<T> entity)
    {
        await dbSet.AddRangeAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        dbSet.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var entity = await dbSet.FindAsync(id);
        if (entity != null)
        {
            dbSet.Remove(entity);
            await context.SaveChangesAsync();
        }
    }

    public async Task BatchDeleteAsync(Expression<Func<T, bool>> predicate)
    {
        var entitiesToDelete = await dbSet.Where(predicate).ToListAsync();

        if (entitiesToDelete.Count != 0)
        {
            dbSet.RemoveRange(entitiesToDelete);
            await context.SaveChangesAsync();
        }
    }
}