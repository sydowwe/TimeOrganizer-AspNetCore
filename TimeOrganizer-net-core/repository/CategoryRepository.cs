using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository.abs;

namespace TimeOrganizer_net_core.repository;

public interface ICategoryRepository : IRepository<Category>
{
}

public class CategoryRepository(AppDbContext context) : ParentRepository<Category>(context), ICategoryRepository;