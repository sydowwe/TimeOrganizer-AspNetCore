using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TimeOrganizer_net_core.model.entity.abs;

public abstract class AbstractEntityWithUser : AbstractEntity
{
    [Required]
    public long userId { get; set; }

    [ForeignKey(nameof(userId))]
    public virtual User user { get; set; }

    protected AbstractEntityWithUser()
    {
    }

    protected AbstractEntityWithUser(long userId)
    {
        this.userId = userId;
    }
}
