namespace TimeOrganizer_net_core.model.DTO.response.generic;

public interface IIdResponse : IResponse
{
    public long Id { get; set; }
}
public class IdResponse : IIdResponse
{
    public long Id { get; set; }

    public IdResponse()
    {
    }

    public IdResponse(long id)
    {
        this.Id = id;
    }
}
