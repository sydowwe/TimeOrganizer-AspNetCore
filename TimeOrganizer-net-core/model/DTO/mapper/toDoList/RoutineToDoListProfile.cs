using AutoMapper;
using TimeOrganizer_net_core.model.DTO.request.ToDoList;
using TimeOrganizer_net_core.model.DTO.response.toDoList;
using TimeOrganizer_net_core.model.entity;

namespace TimeOrganizer_net_core.model.DTO.mapper.toDoList;

public class RoutineToDoListProfile : Profile
{
    public RoutineToDoListProfile()
    {
        CreateMap<RoutineToDoListRequest, RoutineToDoList>();
        CreateMap<RoutineToDoList, RoutineToDoListResponse>();
    }
}