using TimeOrganizer_net_core.model.DTO.request.history;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository.abs;

namespace TimeOrganizer_net_core.repository;

public interface IActivityHistoryRepository : IEntityWithActivityRepository<ActivityHistory>
{
    IQueryable<ActivityHistory> applyFilters(long userId, ActivityHistoryFilterRequest filter);
}

public class ActivityHistoryRepository(AppDbContext context) : EntityWithActivityRepository<ActivityHistory>(context), IActivityHistoryRepository
{
    public IQueryable<ActivityHistory> applyFilters(long userId, ActivityHistoryFilterRequest filter)
    {
        var query = context.activityHistories.AsQueryable();
        query = query.Where(h => h.userId == userId);

        if (filter.activityId.HasValue)
            query = query.Where(h => h.activityId == filter.activityId);

        if (filter.roleId.HasValue)
            query = query.Where(h => h.activity.categoryId == filter.roleId);

        if (filter.categoryId.HasValue)
            query = query.Where(h => h.activity.roleId == filter.categoryId);

        if (filter.isFromToDoList.HasValue)
            query = query.Where(h => h.activity.isOnToDoList == filter.isFromToDoList);

        if (filter.isUnavoidable.HasValue)
            query = query.Where(h => h.activity.isUnavoidable == filter.isUnavoidable);

        if (filter.dateFrom.HasValue)
            query = query.Where(h => h.startTimestamp >= filter.dateFrom);

        if (filter.dateTo.HasValue)
            query = query.Where(h => h.startTimestamp <= filter.dateTo);

        if (filter.hoursBack.HasValue)
        {
            var cutoffTime = DateTime.UtcNow.AddHours(-filter.hoursBack.Value);
            query = query.Where(h => h.startTimestamp >= cutoffTime);
        }

        return query;
    }
}