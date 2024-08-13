using AutoMapper;
using TimeOrganizer_net_core.model.DTO.request.plannerTask;
using TimeOrganizer_net_core.model.DTO.response;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository;
using TimeOrganizer_net_core.service.abs;

namespace TimeOrganizer_net_core.service;

public interface IPlannerTaskService : IEntityWithIsDoneService<PlannerTask, PlannerTaskRequest, PlannerTaskResponse>
{
    Task<List<PlannerTaskResponse>> getAllByDateAndHourSpan(PlannerFilterRequest request);
}

public class PlannerTaskService(IPlannerTaskRepository repository,IActivityRepository activityRepository, IUserRepository userRepository, IMapper mapper)
    : EntityWithIsDoneService<PlannerTask, PlannerTaskRequest, PlannerTaskResponse, IPlannerTaskRepository>(repository, activityRepository, userRepository, mapper), IPlannerTaskService
{
    public async Task<List<PlannerTaskResponse>> getAllByDateAndHourSpan(PlannerFilterRequest request)
    {
        var userId = 0; //userService.GetLoggedUser().Id;
        var endDate = request.filterDate.AddSeconds(request.hourSpan * 3600);
        
        var tasks = await repository.getAllByDateAndHourSpan(userId, request.filterDate, endDate);
        return mapper.Map<List<PlannerTaskResponse>>(tasks);
    }
};