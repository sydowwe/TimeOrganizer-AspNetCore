using TimeOrganizer_net_core.model.DTO.response.extendable;

namespace TimeOrganizer_net_core.model.DTO.response;

public class PlannerTaskResponse : WithIsDoneResponse
{
    private DateTime startTimestamp { get; set; }
    private int minuteLength { get; set; }
}