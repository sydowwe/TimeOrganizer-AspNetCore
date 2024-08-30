using TimeOrganizer_net_core.model.DTO.response.generic;

namespace TimeOrganizer_net_core.model.DTO.response.activity;

public class ActivitySelectOptionResponse : SelectOptionResponse
{
    public long RoleId { get; set; }
    public long? CategoryId { get; set; }
}