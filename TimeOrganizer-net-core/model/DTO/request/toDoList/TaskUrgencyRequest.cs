using TimeOrganizer_net_core.model.DTO.request.extendable;

namespace TimeOrganizer_net_core.model.DTO.request.ToDoList;

public class TaskUrgencyRequest : TextColorRequest
{
    public int priority { get; set; }
}
