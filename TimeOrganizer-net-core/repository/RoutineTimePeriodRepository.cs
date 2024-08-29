using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository.abs;

namespace TimeOrganizer_net_core.repository;

public interface IRoutineTimePeriodRepository : IRepository<RoutineTimePeriod>
{
    Task ChangeIsHiddenInViewAsync(long id);
}

public class RoutineTimePeriodRepository(AppDbContext context) : ParentRepository<RoutineTimePeriod>(context), IRoutineTimePeriodRepository
{
    public async Task ChangeIsHiddenInViewAsync(long id)
    {
        var timePeriod = await dbSet.FindAsync(id);
        if (timePeriod !=null)
        {
            timePeriod.IsHiddenInView = !timePeriod.IsHiddenInView;
            await context.SaveChangesAsync();
        }
    }
}