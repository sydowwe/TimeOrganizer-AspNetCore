using Microsoft.EntityFrameworkCore;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository.abs;

namespace TimeOrganizer_net_core.repository;

public interface IPlannerTaskRepository : IEntityWithIsDoneRepository<PlannerTask>
{
    Task<IEnumerable<PlannerTask>> GetAllByDateAndHourSpan(long userId, DateTime startDate, DateTime endDate);
}

public class PlannerTaskRepository(AppDbContext context) : EntityWithIsDoneRepository<PlannerTask>(context), IPlannerTaskRepository
{
    public async Task<IEnumerable<PlannerTask>> GetAllByDateAndHourSpan(long userId, DateTime startDate, DateTime endDate)
    {
        return await context.PlannerTasks
            .Where(task => task.UserId == userId && task.StartTimestamp >= startDate && task.StartTimestamp.AddMinutes(task.MinuteLength) <= endDate)
            .ToListAsync();
    }
}