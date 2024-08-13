using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository.abs;

namespace TimeOrganizer_net_core.repository;

public interface IRoutineTimePeriodRepository : IEntityWithUserRepository<RoutineTimePeriod>
{
    Task changeIsHiddenInViewAsync(long id);
}

public class RoutineTimePeriodRepository(AppDbContext context) : EntityWithUserRepository<RoutineTimePeriod>(context), IRoutineTimePeriodRepository
{
    public async Task changeIsHiddenInViewAsync(long id)
    {
        var timePeriod = await dbSet.FindAsync(id);
        if (timePeriod !=null)
        {
            timePeriod.isHiddenInView = !timePeriod.isHiddenInView;
            await context.SaveChangesAsync();
        }
    }
}