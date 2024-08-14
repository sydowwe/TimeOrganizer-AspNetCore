using AutoMapper;
using TimeOrganizer_net_core.model.DTO.request.ToDoList;
using TimeOrganizer_net_core.model.DTO.response.toDoList;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository;
using TimeOrganizer_net_core.service.abs;

namespace TimeOrganizer_net_core.service;

public interface ITaskUrgencyService : IMyService<TaskUrgency, TaskUrgencyRequest, TaskUrgencyResponse>
{
    Task createDefaultItems(long newUserId);
}

public class TaskUrgencyService(ITaskUrgencyRepository repository, IUserService userService, IMapper mapper)
    : MyService<TaskUrgency, TaskUrgencyRequest, TaskUrgencyResponse, ITaskUrgencyRepository>(repository, userService, mapper), ITaskUrgencyService
{
    public async Task createDefaultItems(long newUserId)
    {
        this.repository.addRangeAsync(
            [
                new TaskUrgency(userId, "Today", "#FF5252", 1),         // Red
                new TaskUrgency(userId, "This week", "#FFA726", 2),      // Orange
                new TaskUrgency(userId, "This month", "#FFD600", 3),     // Yellow
                new TaskUrgency(userId, "This year", "#4CAF50", 4)    
            ]
        );
    }
};