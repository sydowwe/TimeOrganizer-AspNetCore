using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TimeOrganizer_net_core.model.entity.abs;

public abstract class AbstractEntityWithActivity : AbstractEntityWithUser
{
    public virtual Activity Activity { get; set; }

    public long ActivityId { get; set; }

    protected AbstractEntityWithActivity()
    {
    }

    protected AbstractEntityWithActivity(long userId) : base(userId)
    {
    }
}
public class AbstractEntityWithActivityConfiguration : IEntityTypeConfiguration<AbstractEntityWithActivity>
{
    public void Configure(EntityTypeBuilder<AbstractEntityWithActivity> builder)
    {
        builder.HasOne(p => p.Activity)
            .WithMany()
            .HasForeignKey(p => p.ActivityId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict); // Adjust cascade behavior as needed
    }
}