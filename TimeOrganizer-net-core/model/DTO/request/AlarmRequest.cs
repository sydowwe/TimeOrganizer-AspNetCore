using TimeOrganizer_net_core.model.DTO.request.activity;
using TimeOrganizer_net_core.model.DTO.request.extendable;

namespace TimeOrganizer_net_core.model.DTO.request;

public class AlarmRequest : NameTextColorRequest, IActivityIdRequest
{
    public DateTime startTimestamp { get; set; }
    public long activityId { get; set; }
    public bool isActive { get; set; }
}