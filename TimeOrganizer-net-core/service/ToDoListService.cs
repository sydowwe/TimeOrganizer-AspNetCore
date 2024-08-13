using AutoMapper;
using TimeOrganizer_net_core.model.DTO.request;
using TimeOrganizer_net_core.model.DTO.request.generic;
using TimeOrganizer_net_core.model.DTO.request.ToDoList;
using TimeOrganizer_net_core.model.DTO.response.toDoList;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository;
using TimeOrganizer_net_core.service.abs;

namespace TimeOrganizer_net_core.service;

public interface IToDoListService : IEntityWithIsDoneService<ToDoList, ToDoListRequest, ToDoListResponse>
{
}

public class ToDoListService(IToDoListRepository repository, IActivityRepository activityRepository, IUserRepository userRepository, IMapper mapper)
    : EntityWithIsDoneService<ToDoList, ToDoListRequest, ToDoListResponse, IToDoListRepository>(repository, activityRepository, userRepository, mapper), IToDoListService
{
};