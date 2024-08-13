using TimeOrganizer_net_core.model.DTO.request.extendable;

namespace TimeOrganizer_net_core.model.DTO.request.ToDoList;

public class TimePeriodRequest : TextColorRequest
{
    public int length { get; set; }
    public bool isHiddenInView { get; set; }
}
