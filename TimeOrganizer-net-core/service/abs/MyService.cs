using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TimeOrganizer_net_core.model.DTO.request;
using TimeOrganizer_net_core.model.DTO.request.extendable;
using TimeOrganizer_net_core.model.DTO.request.generic;
using TimeOrganizer_net_core.model.DTO.response.generic;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.model.entity.abs;
using TimeOrganizer_net_core.repository;
using TimeOrganizer_net_core.repository.abs;

namespace TimeOrganizer_net_core.service.abs;

public interface IMyService<TEntity, in TRequest, TResponse>
{
    Task<TResponse> getByIdAsync(long id);
    Task<IEnumerable<TResponse>> getAllAsync();
    Task<TResponse> insertAsync(TRequest request);
    Task<TResponse> updateAsync(long id, TRequest request);
    Task deleteAsync(long id);
    Task batchDeleteAsync(IEnumerable<IdRequest> requestList);
}

public abstract class MyService<TEntity, TRequest, TResponse, TRepository>(
    TRepository repository,
    IUserService userService,
    IMapper mapper
) : IMyService<TEntity, TRequest, TResponse>
    where TEntity : AbstractEntityWithUser
    where TRequest : IRequest
    where TResponse : IResponse
    where TRepository : IRepository<TEntity>
{
    protected readonly TRepository repository = repository;
    protected readonly IUserService userService = userService;
    protected readonly IMapper mapper = mapper;
    protected long userId = 1;
    
    public async Task<TResponse> getByIdAsync(long id)
    {
        var entity = await repository.getByIdAsync(id);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Entity with id {id} not found.");
        }
        return mapper.Map<TResponse>(entity);
    }

    public async Task<IEnumerable<TResponse>> getAllAsync()
    {
        return await repository.getAsQueryable(userId).ProjectTo<TResponse>(mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<TResponse> insertAsync(TRequest request)
    {
        var entity = mapper.Map<TEntity>(request);
        await repository.addAsync(entity);
        return mapper.Map<TResponse>(entity);
    }

    public async Task<IEnumerable<TResponse>> insertRangeAsync(IEnumerable<TRequest> request)
    {
        var entities = mapper.Map<IEnumerable<TEntity>>(request);
        await repository.addRangeAsync(entities);
        return mapper.Map<IEnumerable<TResponse>>(entities);
    }

    public async Task<TResponse> updateAsync(long id, TRequest request)
    {
        var entity = await repository.getByIdAsync(id);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Entity with id {id} not found.");
        }
        mapper.Map(request, entity);
        await repository.updateAsync(entity);
        return mapper.Map<TResponse>(entity);
    }

    public async Task deleteAsync(long id)
    {
        await repository.deleteAsync(id);
    }

    public async Task batchDeleteAsync(IEnumerable<IdRequest> requestList)
    {
        var ids = requestList.Select(req => req.id);
        await repository.batchDeleteAsync(i => ids.Contains(i.id));
    }
}