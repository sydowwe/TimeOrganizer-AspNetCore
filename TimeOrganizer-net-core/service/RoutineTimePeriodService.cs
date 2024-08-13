using AutoMapper;
using TimeOrganizer_net_core.model.DTO.request.ToDoList;
using TimeOrganizer_net_core.model.DTO.response.toDoList;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository;
using TimeOrganizer_net_core.service.abs;

namespace TimeOrganizer_net_core.service;

public interface IRoutineTimePeriodService : IMyService<RoutineTimePeriod, TimePeriodRequest, TimePeriodResponse>
{
    Task createDefaultItems(long userId);
    Task changeIsHiddenInViewAsync(long id);
}

public class RoutineTimePeriodService(IRoutineTimePeriodRepository repository, IUserRepository userRepository, IMapper mapper)
    : MyService<RoutineTimePeriod, TimePeriodRequest, TimePeriodResponse,IRoutineTimePeriodRepository>(repository, userRepository, mapper), IRoutineTimePeriodService
{
    public async Task createDefaultItems(long userId)
    {
        await this.repository.addRangeAsync(
            [
                new RoutineTimePeriod(userId, "Daily", "#92F58C", 1, false),         // Green
                new RoutineTimePeriod(userId, "Weekly", "#936AF1", 7, false),      // purple
                new RoutineTimePeriod(userId, "Monthly", "#2C7EF4", 30, false),     // blue
                new RoutineTimePeriod(userId, "Yearly", "#A5CCF3", 365, false) 
            ]
        );
    }

    public async Task changeIsHiddenInViewAsync(long id)
    {
        await repository.changeIsHiddenInViewAsync(id);
    }
}