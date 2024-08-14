using Microsoft.EntityFrameworkCore;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository.abs;

namespace TimeOrganizer_net_core.repository;

public interface IPlannerTaskRepository : IEntityWithIsDoneRepository<PlannerTask>
{
    Task<IEnumerable<PlannerTask>> getAllByDateAndHourSpan(long userId, DateTime startDate, DateTime endDate);
}

public class PlannerTaskRepository(AppDbContext context) : EntityWithIsDoneRepository<PlannerTask>(context), IPlannerTaskRepository
{
    public async Task<IEnumerable<PlannerTask>> getAllByDateAndHourSpan(long userId, DateTime startDate, DateTime endDate)
    {
        return await context.plannerTasks
            .Where(task => task.userId == userId && task.startTimestamp >= startDate && task.startTimestamp.AddMinutes(task.minuteLength) <= endDate)
            .ToListAsync();
    }
}