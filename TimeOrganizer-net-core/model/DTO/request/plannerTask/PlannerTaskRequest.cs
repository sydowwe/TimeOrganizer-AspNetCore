using TimeOrganizer_net_core.model.DTO.request.activity;
using TimeOrganizer_net_core.model.DTO.request.extendable;
using TimeOrganizer_net_core.model.DTO.request.ToDoList;

namespace TimeOrganizer_net_core.model.DTO.request.plannerTask;

public class PlannerTaskRequest : WithIsDoneRequest
{
    public DateTime startTimestamp { get; set; }
    public int minuteLength { get; set; }
}
