using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TimeOrganizer_net_core.model.DTO.request.activity;
using TimeOrganizer_net_core.model.DTO.response.activity;
using TimeOrganizer_net_core.model.DTO.response.generic;
using TimeOrganizer_net_core.model.entity.abs;
using TimeOrganizer_net_core.repository;
using TimeOrganizer_net_core.repository.abs;
using TimeOrganizer_net_core.security;

namespace TimeOrganizer_net_core.service.abs;

public interface
    IEntityWithActivityService<TEntity, in TRequest, TResponse> : IMyService<TEntity, TRequest, TResponse>
    where TEntity : AbstractEntityWithActivity
    where TRequest : IActivityIdRequest
    where TResponse : IEntityWithActivityResponse
{
    Task<List<ActivityFormSelectOptionsResponse>> GetAllActivityFormSelectOptions();
}

public abstract class EntityWithActivityService<TEntity, TRequest, TResponse, TRepository>(
    TRepository repository,
    IActivityService activityService,
    ILoggedUserService loggedUserService,
    IMapper mapper
) : MyService<TEntity, TRequest, TResponse, TRepository>(repository, loggedUserService, mapper),
    IEntityWithActivityService<TEntity, TRequest, TResponse>
    where TEntity : AbstractEntityWithActivity
    where TRequest : class, IActivityIdRequest
    where TResponse : class, IEntityWithActivityResponse
    where TRepository : IEntityWithActivityRepository<TEntity>
{
    protected readonly IActivityService activityService = activityService;

    public async Task<List<ActivityFormSelectOptionsResponse>> GetAllActivityFormSelectOptions()
    {
        var test = await ProjectFromQueryToListAsync<ActivityFormSelectOptionsResponse>(
            repository.GetDistinctActivities(GetLoggedUserId()));
        return test;
    }

}