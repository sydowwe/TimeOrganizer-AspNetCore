using TimeOrganizer_net_core.helper;
using TimeOrganizer_net_core.model.DTO.request.activity;

namespace TimeOrganizer_net_core.model.DTO.request.history;

public class HistoryRequest : ActivityIdRequest
{
    public DateTime startTimestamp { get; set; }
    public MyIntTime length { get; set; }
}
