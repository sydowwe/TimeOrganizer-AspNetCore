using TimeOrganizer_net_core.model.DTO.response.extendable;

namespace TimeOrganizer_net_core.model.DTO.response;

public class PlannerTaskResponse : WithIsDoneResponse
{
    private DateTime StartTimestamp { get; set; }
    private int MinuteLength { get; set; }
}