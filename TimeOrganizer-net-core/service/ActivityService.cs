using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TimeOrganizer_net_core.model.DTO.request.activity;
using TimeOrganizer_net_core.model.DTO.request.extendable;
using TimeOrganizer_net_core.model.DTO.response;
using TimeOrganizer_net_core.model.DTO.response.activity;
using TimeOrganizer_net_core.model.DTO.response.generic;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository;
using TimeOrganizer_net_core.security;
using TimeOrganizer_net_core.service.abs;

namespace TimeOrganizer_net_core.service;

public interface IActivityService : IMyService<Activity, ActivityRequest, ActivityResponse>
{
    Task<IEnumerable<ActivitySelectOptionResponse>> GetAllAsOptionsAsync();
}

public class ActivityService(IActivityRepository repository, ILoggedUserService loggedUserService, IMapper mapper)
    : MyService<Activity, ActivityRequest, ActivityResponse, IActivityRepository>(repository, loggedUserService, mapper),
        IActivityService
{
    //TODO make activityForm selects methods
    public new async Task<IEnumerable<ActivitySelectOptionResponse>> GetAllAsOptionsAsync()
    {
        return await repository.GetAsQueryable(loggedUserService.GetLoggedUserId()).ProjectTo<ActivitySelectOptionResponse>(mapper.ConfigurationProvider).ToListAsync();
    }
    public async Task<ActivityResponse> QuickUpdateAsync(long id, NameTextRequest request)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Entity with id {id} not found.");
        }
        mapper.Map(request, entity);
        await repository.UpdateAsync(entity);
        return mapper.Map<ActivityResponse>(entity);
    }
}
