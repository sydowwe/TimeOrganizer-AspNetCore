using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository.abs;

namespace TimeOrganizer_net_core.repository;

public interface ITaskUrgencyRepository : IRepository<TaskUrgency>
{
}

public class TaskUrgencyRepository(AppDbContext context) : ParentRepository<TaskUrgency>(context), ITaskUrgencyRepository;