using AutoMapper;
using TimeOrganizer_net_core.model.DTO.request.ToDoList;
using TimeOrganizer_net_core.model.DTO.response.toDoList;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository;
using TimeOrganizer_net_core.security;
using TimeOrganizer_net_core.service.abs;

namespace TimeOrganizer_net_core.service;

public interface ITaskUrgencyService : IMyService<TaskUrgency, TaskUrgencyRequest, TaskUrgencyResponse>
{
    Task CreateDefaultItems(long newUserId);
}

public class TaskUrgencyService(ITaskUrgencyRepository repository, ILoggedUserService loggedUserService, IMapper mapper)
    : MyService<TaskUrgency, TaskUrgencyRequest, TaskUrgencyResponse, ITaskUrgencyRepository>(repository, loggedUserService, mapper), ITaskUrgencyService
{
    public async Task CreateDefaultItems(long newUserId)
    {
        await this.repository.AddRangeAsync(
            [
                new TaskUrgency(newUserId, "Today", "#FF5252", 1),         // Red
                new TaskUrgency(newUserId, "This week", "#FFA726", 2),      // Orange
                new TaskUrgency(newUserId, "This month", "#FFD600", 3),     // Yellow
                new TaskUrgency(newUserId, "This year", "#4CAF50", 4)    
            ]
        );
    }
};