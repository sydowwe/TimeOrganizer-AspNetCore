using TimeOrganizer_net_core.helper;
using TimeOrganizer_net_core.model.DTO.response.generic;

namespace TimeOrganizer_net_core.model.DTO.response.history;

public class ActivityHistoryResponse : WithActivityResponse
{
    public DateTime StartTimestamp { get; set; }
    public MyIntTime Length { get; set; }
    public DateTime EndTimestamp { get; set; }
}