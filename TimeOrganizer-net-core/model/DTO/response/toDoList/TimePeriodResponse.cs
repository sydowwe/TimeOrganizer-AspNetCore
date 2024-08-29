using TimeOrganizer_net_core.model.DTO.response.generic;

namespace TimeOrganizer_net_core.model.DTO.response.toDoList;

public class TimePeriodResponse : TextColorResponse
{
    public int LengthInDays { get; set; }
    public bool IsHiddenInView { get; set; }
}