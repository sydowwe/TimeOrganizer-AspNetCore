using TimeOrganizer_net_core.model.DTO.request.extendable;

namespace TimeOrganizer_net_core.model.DTO.request.user;

public class EmailRequest : IRequest
{
    public string Email { get; set; }
}
