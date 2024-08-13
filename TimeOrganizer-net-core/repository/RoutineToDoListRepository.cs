using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository.abs;

namespace TimeOrganizer_net_core.repository;

public interface IRoutineToDoListRepository : IEntityWithIsDoneRepository<RoutineToDoList>
{
    
}

public class RoutineToDoListRepository(AppDbContext context)
    : EntityWithIsDoneRepository<RoutineToDoList>(context), IRoutineToDoListRepository
{
}