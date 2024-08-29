using TimeOrganizer_net_core.model.entity.abs;

namespace TimeOrganizer_net_core.repository.abs;


public interface IEntityWithIsDoneRepository<T> : IEntityWithActivityRepository<T> where T : AbstractEntityWithIsDone
{
    Task<int> UpdateIsDoneByIdsAsync(IEnumerable<long> ids);
}

public class EntityWithIsDoneRepository<T>(AppDbContext context) : EntityWithActivityRepository<T>(context), IEntityWithIsDoneRepository<T>
    where T : AbstractEntityWithIsDone
{
    public async Task<int> UpdateIsDoneByIdsAsync(IEnumerable<long> ids)
    {
        var items = dbSet
            .Where(item => ids.Contains(item.Id))
            .ToList();
        foreach (var item in items)
        {
            item.IsDone = !item.IsDone;
        }
        return await context.SaveChangesAsync();
    }
}