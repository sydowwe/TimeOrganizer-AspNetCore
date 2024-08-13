using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TimeOrganizer_net_core.model.entity.abs;

public abstract class AbstractEntityWithActivity : AbstractEntityWithUser
{
    [ForeignKey(nameof(activityId))]
    public virtual Activity activity { get; set; }

    [Required]
    public long activityId { get; set; }

    protected AbstractEntityWithActivity()
    {
    }

    protected AbstractEntityWithActivity(long userId) : base(userId)
    {
    }
}