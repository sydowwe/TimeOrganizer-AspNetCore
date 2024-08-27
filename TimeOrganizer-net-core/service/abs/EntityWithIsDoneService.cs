using AutoMapper;
using TimeOrganizer_net_core.model.DTO.request.extendable;
using TimeOrganizer_net_core.model.DTO.request.generic;
using TimeOrganizer_net_core.model.DTO.response.extendable;
using TimeOrganizer_net_core.model.entity.abs;
using TimeOrganizer_net_core.repository;
using TimeOrganizer_net_core.repository.abs;
using TimeOrganizer_net_core.security;

namespace TimeOrganizer_net_core.service.abs;

public interface IEntityWithIsDoneService<TEntity, in TRequest, TResponse>
    : IEntityWithActivityService<TEntity, TRequest, TResponse>
    where TEntity : AbstractEntityWithIsDone
    where TRequest : WithIsDoneRequest
    where TResponse : WithIsDoneResponse
{
    Task setIsDoneAsync(IEnumerable<IdRequest> requestList);
}

public abstract class EntityWithIsDoneService<TEntity, TRequest, TResponse, TRepository>(
    TRepository repository,
    IActivityService activityService,
    ILoggedUserService loggedUserService,
    IMapper mapper
) : EntityWithActivityService<TEntity, TRequest, TResponse, TRepository>(repository, activityService,loggedUserService, 
        mapper),
    IEntityWithIsDoneService<TEntity, TRequest, TResponse>
    where TEntity : AbstractEntityWithIsDone
    where TRequest : WithIsDoneRequest
    where TResponse : WithIsDoneResponse
    where TRepository : IEntityWithIsDoneRepository<TEntity>
{
    public async Task setIsDoneAsync(IEnumerable<IdRequest> requestList)
    {
        var ids = requestList.Select(req => req.id);
        var affectedRows = await Repository.updateIsDoneByIdsAsync(ids);
        if (affectedRows <= 0)
        {
            //throw new UpdateFailedException();
        }
    }
}