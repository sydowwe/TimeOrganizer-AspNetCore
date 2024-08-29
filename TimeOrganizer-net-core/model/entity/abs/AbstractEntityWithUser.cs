using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TimeOrganizer_net_core.model.entity.abs;

public abstract class AbstractEntityWithUser : AbstractEntity
{
    [Required]
    public long UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }

    protected AbstractEntityWithUser()
    {
    }

    protected AbstractEntityWithUser(long userId)
    {
        this.UserId = userId;
    }
}
