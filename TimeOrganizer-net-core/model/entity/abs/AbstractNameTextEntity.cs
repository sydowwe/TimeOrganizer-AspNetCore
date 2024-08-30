using System.ComponentModel.DataAnnotations;

namespace TimeOrganizer_net_core.model.entity.abs;

public abstract class AbstractNameTextEntity : AbstractEntityWithUser
{
    [Required]
    //Unique
    public string Name { get; set; }

    public string? Text { get; set; }

    protected AbstractNameTextEntity()
    {}

    protected AbstractNameTextEntity(string name, string text, long userId) : base(userId)
    {
        this.Name = name;
        this.Text = text;
    }
}
