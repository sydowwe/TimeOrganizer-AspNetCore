using TimeOrganizer_net_core.model.DTO.response.generic;

namespace TimeOrganizer_net_core.model.DTO.response;

public class WebExtensionDataResponse : WithActivityResponse
{
    public string domain { get; set; }
    public string title { get; set; }
    public int duration { get; set; }
    public DateTime startTimestamp { get; set; }
}
