using TimeOrganizer_net_core.model.DTO.request.extendable;

namespace TimeOrganizer_net_core.model.DTO.request;

public class ActivitySelectForm : IRequest
{
    public long? activityId { get; set; }
    public long? roleId { get; set; }
    public long? categoryId { get; set; }
    public bool? isFromToDoList { get; set; }
    public bool? isUnavoidable { get; set; }
}