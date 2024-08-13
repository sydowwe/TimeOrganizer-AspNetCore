namespace TimeOrganizer_net_core.model.entity.abs;

public abstract class AbstractAbstractNameTextColorEntity : AbstractNameTextEntity
{
    public string color { get; set; }

    protected AbstractAbstractNameTextColorEntity() : base() {}

    protected AbstractAbstractNameTextColorEntity(string name, string text, string color, long userId) 
        : base(name, text, userId)
    {
        this.color = color;
    }
}
