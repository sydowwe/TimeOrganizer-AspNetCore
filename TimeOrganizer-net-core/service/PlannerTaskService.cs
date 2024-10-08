using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TimeOrganizer_net_core.model.DTO.request.plannerTask;
using TimeOrganizer_net_core.model.DTO.response;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository;
using TimeOrganizer_net_core.security;
using TimeOrganizer_net_core.service.abs;

namespace TimeOrganizer_net_core.service;

public interface IPlannerTaskService : IEntityWithIsDoneService<PlannerTask, PlannerTaskRequest, PlannerTaskResponse>
{
    Task<List<PlannerTaskResponse>> GetAllByDateAndHourSpan(PlannerFilterRequest request);
}

public class PlannerTaskService(IPlannerTaskRepository repository,IActivityService activityService, ILoggedUserService loggedUserService, IMapper mapper)
    : EntityWithIsDoneService<PlannerTask, PlannerTaskRequest, PlannerTaskResponse, IPlannerTaskRepository>(repository, activityService, loggedUserService, mapper), IPlannerTaskService
{
    public async Task<List<PlannerTaskResponse>> GetAllByDateAndHourSpan(PlannerFilterRequest request)
    {
        var endDate = request.FilterDate.AddSeconds(request.HourSpan * 3600); 
        return await ProjectFromQueryToListAsync<PlannerTaskResponse>(repository.GetAllByDateAndHourSpan(loggedUserService.GetLoggedUserId(), request.FilterDate, endDate));
    }
};