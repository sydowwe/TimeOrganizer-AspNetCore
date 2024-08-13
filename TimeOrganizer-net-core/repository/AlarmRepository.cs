using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository.abs;

namespace TimeOrganizer_net_core.repository;

public interface IAlarmRepository: IEntityWithActivityRepository<Alarm>
{
    Task<int> updateIsActiveByIds(IEnumerable<long> ids);
}

public class AlarmRepository(AppDbContext context) : EntityWithActivityRepository<Alarm>(context), IAlarmRepository
{
    public async Task<int> updateIsActiveByIds(IEnumerable<long> ids)
    {
        var alarms = context.alarms
            .Where(alarm => ids.Contains(alarm.id))
            .ToList();
        foreach (var alarm in alarms)
        {
            alarm.isActive = !alarm.isActive;
        }
        return await context.SaveChangesAsync();
    }
}