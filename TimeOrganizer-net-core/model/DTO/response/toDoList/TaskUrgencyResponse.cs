using TimeOrganizer_net_core.model.DTO.response.generic;

namespace TimeOrganizer_net_core.model.DTO.response.toDoList;

public class TaskUrgencyResponse : TextColorResponse
{
    public int priority { get; set; }
}