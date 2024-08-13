using TimeOrganizer_net_core.model.DTO.request.extendable;

namespace TimeOrganizer_net_core.model.DTO.request.ToDoList;

public class ToDoListRequest : WithIsDoneRequest
{
    public long urgencyId { get; set; }
}
