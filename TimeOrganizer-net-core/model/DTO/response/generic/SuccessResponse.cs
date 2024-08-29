namespace TimeOrganizer_net_core.model.DTO.response.generic;

public class SuccessResponse
{
    public string Message { get; set; }
    public string Status => "success";

    public SuccessResponse(string message)
    {
        this.Message = message;
    }
}