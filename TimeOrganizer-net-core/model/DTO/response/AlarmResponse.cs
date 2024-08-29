using TimeOrganizer_net_core.model.DTO.response.generic;

namespace TimeOrganizer_net_core.model.DTO.response;

public class AlarmResponse : NameTextColorResponse, IEntityWithActivityResponse
{
    public DateTime StartTimestamp { get; set; }
    public ActivityResponse Activity { get; set; }
    public bool IsActive { get; set; }
}
