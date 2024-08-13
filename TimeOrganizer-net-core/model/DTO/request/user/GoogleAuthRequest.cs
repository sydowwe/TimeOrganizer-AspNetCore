using TimeOrganizer_net_core.model.DTO.request.generic;

namespace TimeOrganizer_net_core.model.DTO.request.user;

public class GoogleAuthRequest : IdRequest
{
    public string code { get; set; }
}
