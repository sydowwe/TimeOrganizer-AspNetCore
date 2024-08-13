using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository.abs;

namespace TimeOrganizer_net_core.repository;

public interface IRoleRepository : IEntityWithUserRepository<Role>
{
}

public class RoleRepository(AppDbContext context) : EntityWithUserRepository<Role>(context), IRoleRepository;