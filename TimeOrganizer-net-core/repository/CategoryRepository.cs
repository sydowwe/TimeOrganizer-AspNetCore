using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository.abs;

namespace TimeOrganizer_net_core.repository;

public interface ICategoryRepository : IEntityWithUserRepository<Category>
{
}

public class CategoryRepository(AppDbContext context) : EntityWithUserRepository<Category>(context), ICategoryRepository;