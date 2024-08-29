using TimeOrganizer_net_core.helper;
using TimeOrganizer_net_core.model.DTO.request.activity;

namespace TimeOrganizer_net_core.model.DTO.request.history;

public class ActivityHistoryRequest : ActivityIdRequest
{
    public DateTime StartTimestamp { get; set; }
    public MyIntTime Length { get; set; }
}
