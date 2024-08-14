using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository.abs;

namespace TimeOrganizer_net_core.repository;

public interface IRoutineTimePeriodRepository : IRepository<RoutineTimePeriod>
{
    Task changeIsHiddenInViewAsync(long id);
}

public class RoutineTimePeriodRepository(AppDbContext context) : ParentRepository<RoutineTimePeriod>(context), IRoutineTimePeriodRepository
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