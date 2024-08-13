using System.ComponentModel.DataAnnotations.Schema;
using TimeOrganizer_net_core.model.entity.abs;

namespace TimeOrganizer_net_core.model.entity;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RoutineToDoList : AbstractEntityWithIsDone
{
    [ForeignKey("timePeriodId")]
    public virtual RoutineTimePeriod timePeriod { get; set; }
    public long timePeriodId { get; set; }
}
public class RoutineToDoListConfiguration : IEntityTypeConfiguration<RoutineToDoList>
{
    public void Configure(EntityTypeBuilder<RoutineToDoList> builder)
    {
        builder.ToTable("RoutineToDoList", schema: "public");

        // Define unique constraint on (user_id, activity_id)
        builder.HasIndex(r => new { r.userId, r.activityId })
            .IsUnique();

        // Define index on (user_id, time_period_id)
        builder.HasIndex(r => new { r.userId, r.timePeriodId });

        builder.HasOne(r => r.timePeriod)
            .WithMany()
            .HasForeignKey(r => r.timePeriodId)
            .OnDelete(DeleteBehavior.Restrict); // Adjust cascade behavior as needed
    }
}