using TimeOrganizer_net_core.helper;
using TimeOrganizer_net_core.model.DTO.response.generic;

namespace TimeOrganizer_net_core.model.DTO.response.history;

public class HistoryResponse : WithActivityResponse
{
    public DateTime startTimestamp { get; set; }
    public MyIntTime length { get; set; }
    public DateTime endTimestamp { get; set; }
}