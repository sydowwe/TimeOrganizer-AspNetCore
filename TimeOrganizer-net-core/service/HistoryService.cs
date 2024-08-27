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

public interface IHistoryService : IEntityWithActivityService<History, HistoryRequest, HistoryResponse>
{
    Task<List<HistoryListGroupedByDateResponse>> filterAsync(HistoryFilterRequest filterRequest);
}

public class HistoryService(IHistoryRepository repository, ILoggedUserService loggedUserService, IMapper mapper)
    : MyService<History, HistoryRequest, HistoryResponse, IHistoryRepository>(repository, loggedUserService, mapper), IHistoryService
{
       public async Task<List<HistoryListGroupedByDateResponse>> filterAsync(HistoryFilterRequest filterRequest)
    {
        var query = Repository.applyFilters(loggedUserService.GetLoggedUserId(), filterRequest);

        var historyResponses = await query.OrderBy(h=>h.startTimestamp)
            .ProjectTo<HistoryResponse>(Mapper.ConfigurationProvider).ToListAsync();

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