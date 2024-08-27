using AutoMapper;
using TimeOrganizer_net_core.model.DTO.request.ToDoList;
using TimeOrganizer_net_core.model.DTO.response.toDoList;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository;
using TimeOrganizer_net_core.security;
using TimeOrganizer_net_core.service.abs;

namespace TimeOrganizer_net_core.service;

public interface IToDoListService : IEntityWithIsDoneService<ToDoList, ToDoListRequest, ToDoListResponse>
{
}

public class ToDoListService(IToDoListRepository repository, IActivityService activityService, ILoggedUserService loggedUserService, IMapper mapper)
    : EntityWithIsDoneService<ToDoList, ToDoListRequest, ToDoListResponse, IToDoListRepository>(repository, activityService, loggedUserService, mapper), IToDoListService
{
};