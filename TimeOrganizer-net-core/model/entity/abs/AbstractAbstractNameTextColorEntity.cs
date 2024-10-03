using System.ComponentModel.DataAnnotations;

namespace TimeOrganizer_net_core.model.entity.abs;

public abstract class AbstractAbstractNameTextColorEntity : AbstractNameTextEntity
{
    [StringLength(7)]
    public string Color { get; set; } = "1A237E";

    protected AbstractAbstractNameTextColorEntity() : base() {}

    protected AbstractAbstractNameTextColorEntity(string name, string text, string color, long userId) 
        : base(name, text, userId)
    {
        this.Color = color;
    }
}
