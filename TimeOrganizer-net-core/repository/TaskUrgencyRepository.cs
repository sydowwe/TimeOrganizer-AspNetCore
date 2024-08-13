using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository.abs;

namespace TimeOrganizer_net_core.repository;

public interface ITaskUrgencyRepository : IEntityWithUserRepository<TaskUrgency>
{
}

public class TaskUrgencyRepository(AppDbContext context) : EntityWithUserRepository<TaskUrgency>(context), ITaskUrgencyRepository;