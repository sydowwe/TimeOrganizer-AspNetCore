using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TimeOrganizer_net_core.model.DTO.request;
using TimeOrganizer_net_core.model.DTO.request.extendable;
using TimeOrganizer_net_core.model.DTO.response.generic;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository;
using TimeOrganizer_net_core.security;
using TimeOrganizer_net_core.service.abs;

namespace TimeOrganizer_net_core.service;

public interface IRoleService : IMyService<Role, NameTextColorIconRequest, NameTextColorIconResponse>
{
    Task CreateDefaultItems(long newUserId);
    Task<NameTextColorIconResponse?> GetByNameAsync(string name);
}

public class RoleService(IRoleRepository repository, ILoggedUserService loggedUserService, IMapper mapper)
    : MyService<Role, NameTextColorIconRequest, NameTextColorIconResponse,IRoleRepository>(repository, loggedUserService, mapper), IRoleService
{
    //TODO Spravit ako serviceResult
    public async Task<NameTextColorIconResponse?> GetByNameAsync(string name)
    {
        return await repository.GetAsQueryable(loggedUserService.GetLoggedUserId()).Where(r=>r.Name.Equals(name)).ProjectTo<NameTextColorIconResponse>(mapper.ConfigurationProvider).FirstOrDefaultAsync();
    }
    public async Task CreateDefaultItems(long newUserId)
    {
        await this.repository.AddRangeAsync(
            [
                new Role(newUserId, "Planner task", "Quickly created activities in task planner", "", "calendar-days"),
                new Role(newUserId, "To-do list task", "Quickly created activities in to-do list", "", "list-check"),
                new Role(newUserId, "Routine task", "Quickly created activities in routine to-do list", "", "recycle")
            ]
        );
    }
};