namespace TimeOrganizer_net_core.model.DTO.response.generic;

public class SuccessResponse
{
    public string message { get; set; }
    public string status => "success";

    public SuccessResponse(string message)
    {
        this.message = message;
    }
}