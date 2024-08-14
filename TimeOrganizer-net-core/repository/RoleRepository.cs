using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository.abs;

namespace TimeOrganizer_net_core.repository;

public interface IRoleRepository : IRepository<Role>
{
}

public class RoleRepository(AppDbContext context) : ParentRepository<Role>(context), IRoleRepository;