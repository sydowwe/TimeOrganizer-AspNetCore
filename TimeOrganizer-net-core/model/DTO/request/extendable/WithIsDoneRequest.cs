using TimeOrganizer_net_core.model.DTO.request.activity;

namespace TimeOrganizer_net_core.model.DTO.request.extendable;

public class WithIsDoneRequest : ActivityIdRequest
{
    public bool IsDone { get; set; } = false;
}
