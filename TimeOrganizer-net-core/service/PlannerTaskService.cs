using AutoMapper;
using TimeOrganizer_net_core.model.DTO.request.plannerTask;
using TimeOrganizer_net_core.model.DTO.response;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository;
using TimeOrganizer_net_core.security;
using TimeOrganizer_net_core.service.abs;

namespace TimeOrganizer_net_core.service;

public interface IPlannerTaskService : IEntityWithIsDoneService<PlannerTask, PlannerTaskRequest, PlannerTaskResponse>
{
    Task<List<PlannerTaskResponse>> getAllByDateAndHourSpan(PlannerFilterRequest request);
}

public class PlannerTaskService(IPlannerTaskRepository repository,IActivityService activityService, ILoggedUserService loggedUserService, IMapper mapper)
    : EntityWithIsDoneService<PlannerTask, PlannerTaskRequest, PlannerTaskResponse, IPlannerTaskRepository>(repository, activityService, loggedUserService, mapper), IPlannerTaskService
{
    public async Task<List<PlannerTaskResponse>> getAllByDateAndHourSpan(PlannerFilterRequest request)
    {
        var endDate = request.filterDate.AddSeconds(request.hourSpan * 3600);
        var tasks = await Repository.getAllByDateAndHourSpan(loggedUserService.GetLoggedUserId(), request.filterDate, endDate);
        return Mapper.Map<List<PlannerTaskResponse>>(tasks);
    }
};