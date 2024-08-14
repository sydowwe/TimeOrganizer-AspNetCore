using TimeOrganizer_net_core.model.DTO.response.generic;

namespace TimeOrganizer_net_core.model.DTO.response.toDoList;

public class RoutineToDoListGroupedResponse : IResponse
{
    public TimePeriodResponse timePeriod { get; set; }
    public IEnumerable<RoutineToDoListResponse> items { get; set; }

    public RoutineToDoListGroupedResponse(TimePeriodResponse timePeriod, IEnumerable<RoutineToDoListResponse> items)
    {
        this.timePeriod = timePeriod;
        this.items = items;
    }
}