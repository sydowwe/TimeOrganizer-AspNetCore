namespace TimeOrganizer_net_core.model.DTO.response.generic;

public interface IEntityWithActivityResponse : IIdResponse
{
    public ActivityResponse activity { get; set; }
}
public class WithActivityResponse : IdResponse, IEntityWithActivityResponse
{
    public ActivityResponse activity { get; set; }
}