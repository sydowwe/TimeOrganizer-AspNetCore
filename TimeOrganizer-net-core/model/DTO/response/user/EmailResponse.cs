using TimeOrganizer_net_core.model.DTO.response.generic;

namespace TimeOrganizer_net_core.model.DTO.response.user;

public class EmailResponse : IResponse
{
    public string Email { get; set; }
}