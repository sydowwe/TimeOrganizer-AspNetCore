using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TimeOrganizer_net_core.model.DTO.response.generic;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.model.entity.abs;

namespace TimeOrganizer_net_core.repository.abs;

public interface IEntityWithActivityRepository<T> : IRepository<T>
    where T : AbstractEntityWithActivity
{
    Task<List<T>> GetByActivityIdAsync(long userId, long activityId);
    public IQueryable<T> GetByActivityIdAsQueryable(long userId,long activityId);
}

public class EntityWithActivityRepository<T>(AppDbContext context)
    : ParentRepository<T>(context), IEntityWithActivityRepository<T>
    where T : AbstractEntityWithActivity
{
    public async Task<List<T>> GetByActivityIdAsync(long userId, long activityId)
    {
        return await dbSet.Where(e => e.ActivityId == activityId && e.UserId == userId).ToListAsync();
    }

    public IQueryable<T> GetByActivityIdAsQueryable(long userId, long activityId)
    {
        return dbSet.Where(e => e.ActivityId == activityId && e.UserId == userId).AsQueryable();
    }
}