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
using TimeOrganizer_net_core.security;

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
    ILoggedUserService loggedUserService, 
    IMapper mapper
) : IMyService<TEntity, TRequest, TResponse>
    where TEntity : AbstractEntityWithUser
    where TRequest : IRequest
    where TResponse : IResponse
    where TRepository : IRepository<TEntity>
{
    protected readonly TRepository Repository = repository;
    protected readonly IMapper Mapper = mapper;
    
    public async Task<TResponse> getByIdAsync(long id)
    {
        var entity = await Repository.getByIdAsync(id);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Entity with id {id} not found.");
        }
        return Mapper.Map<TResponse>(entity);
    }

    public async Task<IEnumerable<TResponse>> getAllAsync()
    {
        return await Repository.getAsQueryable(loggedUserService.GetLoggedUserId()).ProjectTo<TResponse>(Mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<TResponse> insertAsync(TRequest request)
    {
        var entity = Mapper.Map<TEntity>(request);
        await Repository.addAsync(entity);
        return Mapper.Map<TResponse>(entity);
    }

    public async Task<IEnumerable<TResponse>> insertRangeAsync(IEnumerable<TRequest> request)
    {
        var entities = Mapper.Map<IEnumerable<TEntity>>(request);
        await Repository.addRangeAsync(entities);
        return Mapper.Map<IEnumerable<TResponse>>(entities);
    }

    public async Task<TResponse> updateAsync(long id, TRequest request)
    {
        var entity = await Repository.getByIdAsync(id);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Entity with id {id} not found.");
        }
        Mapper.Map(request, entity);
        await Repository.updateAsync(entity);
        return Mapper.Map<TResponse>(entity);
    }

    public async Task deleteAsync(long id)
    {
        await Repository.deleteAsync(id);
    }

    public async Task batchDeleteAsync(IEnumerable<IdRequest> requestList)
    {
        var ids = requestList.Select(req => req.id);
        await Repository.batchDeleteAsync(i => ids.Contains(i.id));
    }
}