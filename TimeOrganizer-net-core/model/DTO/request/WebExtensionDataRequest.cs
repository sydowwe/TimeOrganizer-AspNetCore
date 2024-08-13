using TimeOrganizer_net_core.model.DTO.request.activity;

namespace TimeOrganizer_net_core.model.DTO.request;

public class WebExtensionDataRequest : ActivityIdRequest
{
    public string domain { get; set; }
    public string title { get; set; }
    public int duration { get; set; }
    public DateTime startTimestamp { get; set; }
}
