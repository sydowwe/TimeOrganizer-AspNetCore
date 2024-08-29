using TimeOrganizer_net_core.model.DTO.request.activity;
using TimeOrganizer_net_core.model.DTO.request.extendable;
using TimeOrganizer_net_core.model.DTO.request.ToDoList;

namespace TimeOrganizer_net_core.model.DTO.request.plannerTask;

public class PlannerTaskRequest : WithIsDoneRequest
{
    public DateTime StartTimestamp { get; set; }
    public int MinuteLength { get; set; }
}
