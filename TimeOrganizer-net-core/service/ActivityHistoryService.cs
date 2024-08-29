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
    Task<List<ActivityHistoryListGroupedByDateResponse>> filterAsync(ActivityHistoryFilterRequest filterRequest);
}

public class ActivityHistoryService(IActivityHistoryRepository repository, ILoggedUserService loggedUserService, IMapper mapper)
    : MyService<ActivityHistory, ActivityHistoryRequest, ActivityHistoryResponse, IActivityHistoryRepository>(repository, loggedUserService, mapper), IActivityHistoryService
{
       public async Task<List<ActivityHistoryListGroupedByDateResponse>> filterAsync(ActivityHistoryFilterRequest filterRequest)
    {
        var query = Repository.applyFilters(loggedUserService.GetLoggedUserId(), filterRequest);

        var historyResponses = await query.OrderBy(h=>h.startTimestamp)
            .ProjectTo<ActivityHistoryResponse>(Mapper.ConfigurationProvider).ToListAsync();

        return historyResponses
            .GroupBy(hr => hr.startTimestamp.ToUniversalTime().Date)
            .Select(group => new ActivityHistoryListGroupedByDateResponse(group.Key, group.OrderBy(h=>h.startTimestamp).ToList()))
            .OrderBy(response => response.date)
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