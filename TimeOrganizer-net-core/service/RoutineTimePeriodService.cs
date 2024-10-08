using AutoMapper;
using TimeOrganizer_net_core.model.DTO.request.ToDoList;
using TimeOrganizer_net_core.model.DTO.response.toDoList;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository;
using TimeOrganizer_net_core.security;
using TimeOrganizer_net_core.service.abs;

namespace TimeOrganizer_net_core.service;

public interface IRoutineTimePeriodService : IMyService<RoutineTimePeriod, TimePeriodRequest, TimePeriodResponse>
{
    Task CreateDefaultItems(long newUserId);
    Task ChangeIsHiddenInViewAsync(long id);
}

public class RoutineTimePeriodService(IRoutineTimePeriodRepository repository, ILoggedUserService loggedUserService, IMapper mapper)
    : MyService<RoutineTimePeriod, TimePeriodRequest, TimePeriodResponse,IRoutineTimePeriodRepository>(repository, loggedUserService, mapper), IRoutineTimePeriodService
{
    public async Task CreateDefaultItems(long newUserId)
    {
        await this.repository.AddRangeAsync(
            [
                new RoutineTimePeriod(newUserId, "Daily", "#92F58C", 1, false),         // Green
                new RoutineTimePeriod(newUserId, "Weekly", "#936AF1", 7, false),      // purple
                new RoutineTimePeriod(newUserId, "Monthly", "#2C7EF4", 30, false),     // blue
                new RoutineTimePeriod(newUserId, "Yearly", "#A5CCF3", 365, false) 
            ]
        );
    }

    public async Task ChangeIsHiddenInViewAsync(long id)
    {
        await repository.ChangeIsHiddenInViewAsync(id);
    }
}