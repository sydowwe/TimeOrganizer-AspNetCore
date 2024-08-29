using TimeOrganizer_net_core.model.DTO.response.generic;

namespace TimeOrganizer_net_core.model.DTO.response.toDoList;

public class RoutineToDoListGroupedResponse : IResponse
{
    public TimePeriodResponse TimePeriod { get; set; }
    public IEnumerable<RoutineToDoListResponse> Items { get; set; }

    public RoutineToDoListGroupedResponse(TimePeriodResponse timePeriod, IEnumerable<RoutineToDoListResponse> items)
    {
        this.TimePeriod = timePeriod;
        this.Items = items;
    }
}