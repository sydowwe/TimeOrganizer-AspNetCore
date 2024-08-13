using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository.abs;

namespace TimeOrganizer_net_core.repository;

public interface IToDoListRepository : IEntityWithIsDoneRepository<ToDoList>
{
}

public class ToDoListRepository(AppDbContext context)
    : EntityWithIsDoneRepository<ToDoList>(context), IToDoListRepository
{
}