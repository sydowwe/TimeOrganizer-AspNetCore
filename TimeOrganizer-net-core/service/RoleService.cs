using AutoMapper;
using TimeOrganizer_net_core.model.DTO.request;
using TimeOrganizer_net_core.model.DTO.request.extendable;
using TimeOrganizer_net_core.model.DTO.response.generic;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository;
using TimeOrganizer_net_core.service.abs;

namespace TimeOrganizer_net_core.service;

public interface IRoleService : IMyService<Role, NameTextColorIconRequest, NameTextColorIconResponse>
{
    Task createDefaultItems(long newUserId);
}

public class RoleService(IRoleRepository repository, IUserService userService, IMapper mapper)
    : MyService<Role, NameTextColorIconRequest, NameTextColorIconResponse,IRoleRepository>(repository, userService, mapper), IRoleService
{
    public async Task createDefaultItems(long newUserId)
    {
        await this.repository.addRangeAsync(
            [
                new Role(userId, "Planner task", "Quickly created activities in task planner", "", "calendar-days"),
                new Role(userId, "To-do list task", "Quickly created activities in to-do list", "", "list-check"),
                new Role(userId, "Routine task", "Quickly created activities in routine to-do list", "", "recycle")
            ]
        );
    }
};