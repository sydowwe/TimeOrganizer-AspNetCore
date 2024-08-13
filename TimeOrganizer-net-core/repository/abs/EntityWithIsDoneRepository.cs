using TimeOrganizer_net_core.model.entity.abs;

namespace TimeOrganizer_net_core.repository.abs;


public interface IEntityWithIsDoneRepository<T> : IEntityWithActivityRepository<T> where T : AbstractEntityWithIsDone
{
    Task<int> updateIsDoneByIdsAsync(IEnumerable<long> ids);
}

public class EntityWithIsDoneRepository<T>(AppDbContext context) : EntityWithActivityRepository<T>(context), IEntityWithIsDoneRepository<T>
    where T : AbstractEntityWithIsDone
{
    public async Task<int> updateIsDoneByIdsAsync(IEnumerable<long> ids)
    {
        var items = dbSet
            .Where(item => ids.Contains(item.id))
            .ToList();
        foreach (var item in items)
        {
            item.isDone = !item.isDone;
        }
        return await context.SaveChangesAsync();
    }
}