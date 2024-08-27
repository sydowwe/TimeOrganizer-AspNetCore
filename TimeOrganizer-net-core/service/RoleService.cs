using AutoMapper;
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
    Task createDefaultItems(long newUserId);
}

public class RoleService(IRoleRepository repository, ILoggedUserService loggedUserService, IMapper mapper)
    : MyService<Role, NameTextColorIconRequest, NameTextColorIconResponse,IRoleRepository>(repository, loggedUserService, mapper), IRoleService
{
    public async Task createDefaultItems(long newUserId)
    {
        await this.Repository.addRangeAsync(
            [
                new Role(loggedUserService.GetLoggedUserId(), "Planner task", "Quickly created activities in task planner", "", "calendar-days"),
                new Role(loggedUserService.GetLoggedUserId(), "To-do list task", "Quickly created activities in to-do list", "", "list-check"),
                new Role(loggedUserService.GetLoggedUserId(), "Routine task", "Quickly created activities in routine to-do list", "", "recycle")
            ]
        );
    }
};