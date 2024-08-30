using TimeOrganizer_net_core.model.DTO.response.generic;

namespace TimeOrganizer_net_core.model.DTO.response.activity;

public class ActivityResponse : NameTextResponse
{
    public bool IsOnToDoList { get; set; }
    public bool IsUnavoidable { get; set; }
    public NameTextColorIconResponse Role { get; set; }
    public NameTextColorIconResponse Category { get; set; }
}
