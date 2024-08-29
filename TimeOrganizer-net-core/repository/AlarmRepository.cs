using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository.abs;

namespace TimeOrganizer_net_core.repository;

public interface IAlarmRepository: IEntityWithActivityRepository<Alarm>
{
    Task<int> UpdateIsActiveByIds(IEnumerable<long> ids);
}

public class AlarmRepository(AppDbContext context) : EntityWithActivityRepository<Alarm>(context), IAlarmRepository
{
    public async Task<int> UpdateIsActiveByIds(IEnumerable<long> ids)
    {
        var alarms = context.Alarms
            .Where(alarm => ids.Contains(alarm.Id))
            .ToList();
        foreach (var alarm in alarms)
        {
            alarm.IsActive = !alarm.IsActive;
        }
        return await context.SaveChangesAsync();
    }
}