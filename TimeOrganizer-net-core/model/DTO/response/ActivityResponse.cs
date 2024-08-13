using TimeOrganizer_net_core.model.DTO.response.generic;

namespace TimeOrganizer_net_core.model.DTO.response;

public class ActivityResponse : NameTextResponse
{
    public bool isOnToDoList { get; set; }
    public bool isUnavoidable { get; set; }
    public NameTextColorIconResponse role { get; set; }
    public NameTextColorIconResponse category { get; set; }
}
