using TimeOrganizer_net_core.model.DTO.request.extendable;

namespace TimeOrganizer_net_core.model.DTO.request;

public class ActivitySelectForm : IRequest
{
    public long? ActivityId { get; set; }
    public long? RoleId { get; set; }
    public long? CategoryId { get; set; }
    public bool? IsFromToDoList { get; set; }
    public bool? IsUnavoidable { get; set; }
}