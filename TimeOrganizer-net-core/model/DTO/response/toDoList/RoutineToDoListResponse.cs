using TimeOrganizer_net_core.model.DTO.response.extendable;

namespace TimeOrganizer_net_core.model.DTO.response.toDoList;

public class RoutineToDoListResponse : WithIsDoneResponse
{
    public TimePeriodResponse timePeriod { get; set; }
}