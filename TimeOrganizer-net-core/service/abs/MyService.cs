using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TimeOrganizer_net_core.model.DTO.request.extendable;
using TimeOrganizer_net_core.model.DTO.request.generic;
using TimeOrganizer_net_core.model.DTO.response.generic;
using TimeOrganizer_net_core.model.entity.abs;
using TimeOrganizer_net_core.repository.abs;
using TimeOrganizer_net_core.security;

namespace TimeOrganizer_net_core.service.abs;

public interface IMyService<TEntity, in TRequest, TResponse>
{
    Task<TResponse> GetByIdAsync(long id);
    Task<List<TResponse>> GetAllAsync();
    Task<List<SelectOptionResponse>> GetAllAsOptionsAsync();
    Task<TResponse> InsertAsync(TRequest request);
    Task<TResponse> UpdateAsync(long id, TRequest request);
    Task DeleteAsync(long id);
    Task BatchDeleteAsync(IEnumerable<IdRequest> requestList);
    Task<List<T>> ProjectFromQueryToListAsync<T>(IQueryable<TEntity> query) where T : class, IResponse;
}

public abstract class MyService<TEntity, TRequest, TResponse, TRepository>(
    TRepository repository,
    ILoggedUserService loggedUserService,
    IMapper mapper
) : IMyService<TEntity, TRequest, TResponse>
    where TEntity : AbstractEntityWithUser
    where TRequest : class, IRequest
    where TResponse : class, IResponse
    where TRepository : IRepository<TEntity>
{
    protected readonly TRepository repository = repository;
    protected readonly IMapper mapper = mapper;
    
    public async Task<TResponse> GetByIdAsync(long id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Entity with id {id} not found.");
        }
        return mapper.Map<TResponse>(entity);
    }

    public async Task<List<TResponse>> GetAllAsync()
    {
        return await ProjectFromQueryToListAsync<TResponse>(repository.GetAsQueryable(loggedUserService.GetLoggedUserId()));
    }
    public virtual async Task<List<SelectOptionResponse>> GetAllAsOptionsAsync()
    {
        return await ProjectFromQueryToListAsync<SelectOptionResponse>(repository.GetAsQueryable(loggedUserService.GetLoggedUserId()));
    }
    public async Task<TResponse> InsertAsync(TRequest request)
    {
        var entity = mapper.Map<TEntity>(request);
        await repository.AddAsync(entity);
        return mapper.Map<TResponse>(entity);
    }

    public async Task<IEnumerable<TResponse>> InsertRangeAsync(IEnumerable<TRequest> request)
    {
        var entities = mapper.Map<List<TEntity>>(request);
        await repository.AddRangeAsync(entities);
        return mapper.Map<IEnumerable<TResponse>>(entities);
    }

    public async Task<TResponse> UpdateAsync(long id, TRequest request)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Entity with id {id} not found.");
        }
        mapper.Map(request, entity);
        await repository.UpdateAsync(entity);
        return mapper.Map<TResponse>(entity);
    }

    public async Task DeleteAsync(long id)
    {
        await repository.DeleteAsync(id);
    }

    public async Task BatchDeleteAsync(IEnumerable<IdRequest> requestList)
    {
        var ids = requestList.Select(req => req.Id);
        await repository.BatchDeleteAsync(i => ids.Contains(i.Id));
    }

    public async Task<List<T>> ProjectFromQueryToListAsync<T>(IQueryable<TEntity> query) where T : class, IResponse
    {
        return await query.ProjectTo<T>(mapper.ConfigurationProvider).ToListAsync();
    }
}