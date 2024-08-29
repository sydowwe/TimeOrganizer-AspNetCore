using TimeOrganizer_net_core.model.DTO.request.extendable;

namespace TimeOrganizer_net_core.model.DTO.request.ToDoList;

public class TimePeriodRequest : TextColorRequest
{
    public int Length { get; set; }
    public bool IsHiddenInView { get; set; }
}
