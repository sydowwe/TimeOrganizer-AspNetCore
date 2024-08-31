using TimeOrganizer_net_core.model.DTO.response.generic;

namespace TimeOrganizer_net_core.model.DTO.response.activity;

public class ActivityFormSelectOptionsResponse : SelectOptionResponse
{
    public SelectOptionResponse RoleOption { get; set; }
    public SelectOptionResponse? CategoryOption { get; set; }
}