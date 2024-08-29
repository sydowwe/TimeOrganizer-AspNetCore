using TimeOrganizer_net_core.model.DTO.response.generic;

namespace TimeOrganizer_net_core.model.DTO.response.user;

public class GoogleAuthResponse : IResponse
{
    public bool Authorized { get; set; }    
}
