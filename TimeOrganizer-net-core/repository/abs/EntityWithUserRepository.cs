using Microsoft.EntityFrameworkCore;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.model.entity.abs;

namespace TimeOrganizer_net_core.repository.abs;

public interface IEntityWithUserRepository<T> : IRepository<T> where T : AbstractEntityWithUser
{
    Task<List<T>> getByUserIdAsync(long userId);
}

public class EntityWithUserRepository<T> : ParentRepository<T>, IEntityWithUserRepository<T> where T : AbstractEntityWithUser
{
    public EntityWithUserRepository(AppDbContext context) : base(context) { }

    public async Task<List<T>> getByUserIdAsync(long userId)
    {
        return await dbSet.Where(e => e.userId == userId).ToListAsync();
    }
}
