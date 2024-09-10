using TimeOrganizer_net_core.model.DTO.request.history;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository.abs;

namespace TimeOrganizer_net_core.repository;

public interface IActivityHistoryRepository : IEntityWithActivityRepository<ActivityHistory>
{
    IQueryable<ActivityHistory> ApplyFilters(long userId, ActivityHistoryFilterRequest filter);
}

public class ActivityHistoryRepository(AppDbContext context)
    : EntityWithActivityRepository<ActivityHistory>(context), IActivityHistoryRepository
{
    public IQueryable<ActivityHistory> ApplyFilters(long userId, ActivityHistoryFilterRequest filter)
    {
        var query = context.ActivityHistories.AsQueryable();
        query = query.Where(h => h.UserId == userId);

        if (filter.ActivityId.HasValue)
            query = query.Where(h => h.ActivityId == filter.ActivityId);
        else
        {
            if (filter.RoleId.HasValue)
                query = query.Where(h => h.Activity.RoleId == filter.RoleId);
            if (filter.CategoryId.HasValue)
                query = query.Where(h => h.Activity.CategoryId == filter.CategoryId);
            if (filter.IsFromToDoList.HasValue)
                query = query.Where(h => h.Activity.IsOnToDoList == filter.IsFromToDoList);
            if (filter.IsUnavoidable.HasValue)
                query = query.Where(h => h.Activity.IsUnavoidable == filter.IsUnavoidable);
        }

        if (filter.DateFrom.HasValue)
        {
            var startOfDateFrom = new DateTime(filter.DateFrom.Value.Year, filter.DateFrom.Value.Month,
                filter.DateFrom.Value.Day, 0, 0, 1, 0, DateTimeKind.Utc);
            query = query.Where(h => h.StartTimestamp >= startOfDateFrom);
        }
        else if (filter.DateTo.HasValue)
        {
            var endOfDateTo = new DateTime(filter.DateTo.Value.Year, filter.DateTo.Value.Month, filter.DateTo.Value.Day,
                23, 59, 59, 999);
            query = query.Where(h => h.StartTimestamp <= endOfDateTo.ToUniversalTime());
            if (filter.HoursBack.HasValue)
            {
                var cutoffTime = endOfDateTo.AddHours(-filter.HoursBack.Value);
                query = query.Where(h => h.StartTimestamp >= cutoffTime.ToUniversalTime());
            }
        }
        return query;
    }
}