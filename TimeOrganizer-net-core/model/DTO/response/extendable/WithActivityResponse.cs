using TimeOrganizer_net_core.model.DTO.response.activity;

namespace TimeOrganizer_net_core.model.DTO.response.generic;

public interface IEntityWithActivityResponse : IIdResponse
{
    public ActivityResponse Activity { get; set; }
}
public class WithActivityResponse : IdResponse, IEntityWithActivityResponse
{
    public ActivityResponse Activity { get; set; }
}