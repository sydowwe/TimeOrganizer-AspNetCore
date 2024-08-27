using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TimeOrganizer_net_core.model.DTO.request.ToDoList;
using TimeOrganizer_net_core.model.DTO.response.toDoList;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository;
using TimeOrganizer_net_core.security;
using TimeOrganizer_net_core.service.abs;

namespace TimeOrganizer_net_core.service;

public interface IRoutineToDoListService : IEntityWithIsDoneService<RoutineToDoList, RoutineToDoListRequest, RoutineToDoListResponse>
{
    Task<IEnumerable<RoutineToDoListGroupedResponse>> getAllGroupedByTimePeriod();
}

public class RoutineToDoListService(
    IRoutineTimePeriodService timePeriodService,
    IRoutineToDoListRepository repository,
    IActivityService activityService,
    ILoggedUserService loggedUserService,
    IMapper mapper)
    : EntityWithIsDoneService<RoutineToDoList, RoutineToDoListRequest, RoutineToDoListResponse, IRoutineToDoListRepository>(repository,
        activityService, loggedUserService, mapper), IRoutineToDoListService
{
    public async Task<IEnumerable<RoutineToDoListGroupedResponse>> getAllGroupedByTimePeriod()
    {
        var allTimePeriods = await timePeriodService.getAllAsync();
        var allItems = await Repository.getAsQueryable(loggedUserService.GetLoggedUserId())
            .ProjectTo<RoutineToDoListResponse>(Mapper.ConfigurationProvider)
            .ToListAsync();
        var groupedItems = allItems.GroupBy(item => item.timePeriod);
        var result = allTimePeriods.Select(tp => new RoutineToDoListGroupedResponse(
            tp, groupedItems.FirstOrDefault(g => g.Key.Equals(tp)) ?? Enumerable.Empty<RoutineToDoListResponse>()
        ));
        return result;
    }
};