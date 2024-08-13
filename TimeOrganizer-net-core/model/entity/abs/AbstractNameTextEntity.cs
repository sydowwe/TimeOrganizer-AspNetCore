using System.ComponentModel.DataAnnotations;

namespace TimeOrganizer_net_core.model.entity.abs;

public abstract class AbstractNameTextEntity : AbstractEntityWithUser
{
    [Required]
    //Unique
    public string name { get; set; }

    public string text { get; set; }

    protected AbstractNameTextEntity()
    {}

    protected AbstractNameTextEntity(string name, string text, long userId) : base(userId)
    {
        this.name = name;
        this.text = text;
    }
}
