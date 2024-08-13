using TimeOrganizer_net_core.model.DTO.request.extendable;

namespace TimeOrganizer_net_core.model.DTO.request.activity;

public class ActivityRequest : NameTextRequest
{
    public bool isOnToDoList { get; set; }
    public bool isUnavoidable { get; set; }
    public long roleId { get; set; }
    public long? categoryId { get; set; }
}