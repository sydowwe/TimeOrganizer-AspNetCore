using TimeOrganizer_net_core.model.DTO.response.generic;

namespace TimeOrganizer_net_core.model.DTO.response.toDoList;

public class RoutineToDoListGroupedResponse : IResponse
{
    public TimePeriodResponse timePeriod { get; set; }
    public List<RoutineToDoListResponse> items { get; set; }
}