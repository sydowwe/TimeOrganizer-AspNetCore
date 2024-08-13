using TimeOrganizer_net_core.model.DTO.response.generic;

namespace TimeOrganizer_net_core.model.DTO.response;

public class AlarmResponse : NameTextColorResponse, IEntityWithActivityResponse
{
    public DateTime startTimestamp { get; set; }
    public ActivityResponse activity { get; set; }
    public bool isActive { get; set; }
}
