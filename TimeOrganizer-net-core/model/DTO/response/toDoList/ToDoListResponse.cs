using TimeOrganizer_net_core.model.DTO.response.extendable;

namespace TimeOrganizer_net_core.model.DTO.response.toDoList;

public class ToDoListResponse : WithIsDoneResponse
{
    public TaskUrgencyResponse taskUrgency { get; set; }
}
