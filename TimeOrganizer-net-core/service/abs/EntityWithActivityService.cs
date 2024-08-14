using AutoMapper;
using TimeOrganizer_net_core.model.DTO.request.activity;
using TimeOrganizer_net_core.model.DTO.response.generic;
using TimeOrganizer_net_core.model.entity.abs;
using TimeOrganizer_net_core.repository;
using TimeOrganizer_net_core.repository.abs;

namespace TimeOrganizer_net_core.service.abs;

public interface
    IEntityWithActivityService<TEntity, in TRequest, TResponse> : IMyService<TEntity, TRequest, TResponse>
    where TEntity : AbstractEntityWithActivity
    where TRequest : IActivityIdRequest
    where TResponse : IEntityWithActivityResponse
{
}

public abstract class EntityWithActivityService<TEntity, TRequest, TResponse, TRepository>(
    TRepository repository,
    IActivityService activityService,
    IUserService userService,
    IMapper mapper
) : MyService<TEntity, TRequest, TResponse, TRepository>(repository, userService, mapper),
    IEntityWithActivityService<TEntity, TRequest, TResponse>
    where TEntity : AbstractEntityWithActivity
    where TRequest : IActivityIdRequest
    where TResponse : IEntityWithActivityResponse
    where TRepository : IEntityWithActivityRepository<TEntity>
{
    protected readonly IActivityService activityService = activityService;
}