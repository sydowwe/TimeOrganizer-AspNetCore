using TimeOrganizer_net_core.model.DTO.request.extendable;

namespace TimeOrganizer_net_core.model.DTO.request.ToDoList;

public class RoutineToDoListRequest : WithIsDoneRequest
{
    public long timePeriodId { get; set; }
}
