using TimeOrganizer_net_core.model.DTO.request.activity;

namespace TimeOrganizer_net_core.model.DTO.request;

public class WebExtensionDataRequest : ActivityIdRequest
{
    public string Domain { get; set; }
    public string Title { get; set; }
    public int Duration { get; set; }
    public DateTime StartTimestamp { get; set; }
}
