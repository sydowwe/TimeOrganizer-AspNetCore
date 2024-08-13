using TimeOrganizer_net_core.model.DTO.request.history;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository.abs;

namespace TimeOrganizer_net_core.repository;

public interface IHistoryRepository : IEntityWithActivityRepository<History>
{
    IQueryable<History> applyFilters(int userId, HistoryFilterRequest filter);
}

public class HistoryRepository(AppDbContext context) : EntityWithActivityRepository<History>(context), IHistoryRepository
{
    public IQueryable<History> applyFilters(int userId, HistoryFilterRequest filter)
    {
        var query = context.histories.AsQueryable();
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