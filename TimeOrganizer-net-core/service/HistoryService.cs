using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TimeOrganizer_net_core.model.DTO.request.history;
using TimeOrganizer_net_core.model.DTO.response.history;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository;
using TimeOrganizer_net_core.service.abs;

namespace TimeOrganizer_net_core.service;

public interface IHistoryService : IEntityWithActivityService<History, HistoryRequest, HistoryResponse>
{
    Task<List<HistoryListGroupedByDateResponse>> filterAsync(HistoryFilterRequest filterRequest);
}

public class HistoryService(IHistoryRepository repository, IUserRepository userRepository, IMapper mapper)
    : MyService<History, HistoryRequest, HistoryResponse, IHistoryRepository>(repository, userRepository, mapper), IHistoryService
{
       public async Task<List<HistoryListGroupedByDateResponse>> filterAsync(HistoryFilterRequest filterRequest)
    {
        // var userId = await userService.GetLoggedUserAsync();
        var userId = 1;
        // Apply filters
        var query = repository.applyFilters(userId, filterRequest);

        var historyResponses = await query
            .Select(h => mapper.Map<HistoryResponse>(h))
            .ToListAsync();

        return historyResponses
            .GroupBy(hr => hr.startTimestamp.ToUniversalTime().Date)
            .Select(group => new HistoryListGroupedByDateResponse(group.Key, group.OrderBy(h=>h.startTimestamp).ToList()))
            .OrderBy(response => response.date)
            .ToList();
    }

    // public async Task<ActivityFormSelectsResponse> UpdateFilterSelectsAsync(ActivitySelectForm request)
    // {
    //     var loggedUserId = (await _userService.GetLoggedUserAsync()).Id;
    //     var query = context.Histories.AsQueryable();
    //
    //     // Apply filters
    //     query = ApplyFilters(query, loggedUserId, request);
    //
    //     var activityList = await query
    //         .Select(h => h.Activity)
    //         .Distinct()
    //         .ToListAsync();
    //
    //     return await activityService.GetActivityFormSelectsFromActivityListAsync(activityList);
    // }
}