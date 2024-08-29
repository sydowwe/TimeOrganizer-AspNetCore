namespace TimeOrganizer_net_core.model.DTO.response.user;

public class LockedOutResponse
{
    public string Status { get; } = "lockedOut";
    public int Seconds { get; set; }
}
