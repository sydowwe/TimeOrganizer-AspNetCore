using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TimeOrganizer_net_core.model.DTO.request.history;
using TimeOrganizer_net_core.model.DTO.response.history;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository;
using TimeOrganizer_net_core.security;
using TimeOrganizer_net_core.service.abs;

namespace TimeOrganizer_net_core.service;

public interface IActivityHistoryService : IEntityWithActivityService<ActivityHistory, ActivityHistoryRequest, ActivityHistoryResponse>
{
    Task<List<ActivityHistoryListGroupedByDateResponse>> FilterAsync(ActivityHistoryFilterRequest filterRequest);
}

public class ActivityHistoryService(IActivityHistoryRepository repository, ILoggedUserService loggedUserService, IMapper mapper)
    : MyService<ActivityHistory, ActivityHistoryRequest, ActivityHistoryResponse, IActivityHistoryRepository>(repository, loggedUserService, mapper), IActivityHistoryService
{
       public async Task<List<ActivityHistoryListGroupedByDateResponse>> FilterAsync(ActivityHistoryFilterRequest filterRequest)
    {
        var query = repository.ApplyFilters(loggedUserService.GetLoggedUserId(), filterRequest);

        var historyResponses = await query.OrderBy(h=>h.StartTimestamp)
            .ProjectTo<ActivityHistoryResponse>(mapper.ConfigurationProvider).ToListAsync();

        return historyResponses
            .GroupBy(hr => hr.StartTimestamp.ToUniversalTime().Date)
            .Select(group => new ActivityHistoryListGroupedByDateResponse(group.Key, group.OrderBy(h=>h.StartTimestamp).ToList()))
            .OrderBy(response => response.Date)
            .ToList();
    }

    // public async Task<ActivityFormSelectsResponse> UpdateFilterSelectsAsync(ActivitySelectForm request)
    // {
    //     var loggedUserId = (await _userService.GetLoggedUserAsync()).Id;
    //     var query = context.Histories.AsQueryable();
    //
    //     // Apply filters
    //     query = applyFilters(query, loggedUserId, request);
    //
    //     var activityList = await query
    //         .Select(h => h.Activity)
    //         .Distinct()
    //         .ToListAsync();
    //
    //     return await activityService.GetActivityFormSelectsFromActivityListAsync(activityList);
    // }
}