using TimeOrganizer_net_core.model.DTO.request.activity;
using TimeOrganizer_net_core.model.DTO.request.extendable;

namespace TimeOrganizer_net_core.model.DTO.request;

public class AlarmRequest : NameTextColorRequest, IActivityIdRequest
{
    public DateTime StartTimestamp { get; set; }
    public long ActivityId { get; set; }
    public bool IsActive { get; set; }
}