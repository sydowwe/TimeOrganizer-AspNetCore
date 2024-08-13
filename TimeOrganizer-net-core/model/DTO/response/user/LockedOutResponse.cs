namespace TimeOrganizer_net_core.model.DTO.response.user;

public class LockedOutResponse
{
    public string status { get; } = "lockedOut";
    public int seconds { get; set; }
}
