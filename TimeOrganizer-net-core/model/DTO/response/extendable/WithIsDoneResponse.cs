using TimeOrganizer_net_core.model.DTO.response.generic;

namespace TimeOrganizer_net_core.model.DTO.response.extendable;

public class WithIsDoneResponse : WithActivityResponse
{
    public bool isDone { get; set; }
}