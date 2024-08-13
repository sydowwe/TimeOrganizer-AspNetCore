using AutoMapper;
using TimeOrganizer_net_core.model.DTO.request.ToDoList;
using TimeOrganizer_net_core.model.DTO.response.toDoList;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository;
using TimeOrganizer_net_core.service.abs;

namespace TimeOrganizer_net_core.service;

public interface IRoutineToDoListService : IEntityWithIsDoneService<RoutineToDoList, RoutineToDoListRequest, RoutineToDoListResponse>
{
}

public class RoutineToDoListService(IRoutineToDoListRepository repository, IActivityRepository activityRepository, IUserRepository userRepository, IMapper mapper)
    : EntityWithIsDoneService<RoutineToDoList, RoutineToDoListRequest, RoutineToDoListResponse, IRoutineToDoListRepository>(repository, activityRepository, userRepository, mapper), IRoutineToDoListService
{
};