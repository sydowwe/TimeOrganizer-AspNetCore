using TimeOrganizer_net_core.model.DTO.response.generic;

namespace TimeOrganizer_net_core.model.DTO.response;

public class WebExtensionDataResponse : WithActivityResponse
{
    public string Domain { get; set; }
    public string Title { get; set; }
    public int Duration { get; set; }
    public DateTime StartTimestamp { get; set; }
}
