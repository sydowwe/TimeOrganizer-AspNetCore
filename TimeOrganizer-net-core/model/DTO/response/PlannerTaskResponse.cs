using TimeOrganizer_net_core.model.DTO.response.extendable;

namespace TimeOrganizer_net_core.model.DTO.response;

public class PlannerTaskResponse : WithIsDoneResponse
{
    public DateTime StartTimestamp { get; set; }
    public int MinuteLength { get; set; }
    public string Color { get; set; }
}