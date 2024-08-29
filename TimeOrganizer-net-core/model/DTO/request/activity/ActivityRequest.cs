using TimeOrganizer_net_core.model.DTO.request.extendable;

namespace TimeOrganizer_net_core.model.DTO.request.activity;

public class ActivityRequest : NameTextRequest
{
    public bool IsOnToDoList { get; set; }
    public bool IsUnavoidable { get; set; }
    public long RoleId { get; set; }
    public long? CategoryId { get; set; }
}