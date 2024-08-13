using Microsoft.EntityFrameworkCore;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.model.entity.abs;

namespace TimeOrganizer_net_core.repository.abs;

public interface IEntityWithActivityRepository<T> : IEntityWithUserRepository<T> where T : AbstractEntityWithActivity
{
    Task<List<T>> getByActivityIdAsync(long activityId, long userId);
}

public class EntityWithActivityRepository<T>(AppDbContext context)
    : EntityWithUserRepository<T>(context), IEntityWithActivityRepository<T>
    where T : AbstractEntityWithActivity
{
    public async Task<List<T>> getByActivityIdAsync(long activityId, long userId)
    {
        return await dbSet.Where(e => e.activityId == activityId && e.userId == userId).ToListAsync();
    }
}