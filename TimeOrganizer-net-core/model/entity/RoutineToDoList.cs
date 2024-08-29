using System.ComponentModel.DataAnnotations.Schema;
using TimeOrganizer_net_core.model.entity.abs;

namespace TimeOrganizer_net_core.model.entity;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RoutineToDoList : AbstractEntityWithIsDone
{
    [ForeignKey("timePeriodId")]
    public virtual RoutineTimePeriod TimePeriod { get; set; }
    public long TimePeriodId { get; set; }
}
public class RoutineToDoListConfiguration : IEntityTypeConfiguration<RoutineToDoList>
{
    public void Configure(EntityTypeBuilder<RoutineToDoList> builder)
    {
        builder.ToTable("RoutineToDoList", schema: "public");

        // Define unique constraint on (user_id, activity_id)
        builder.HasIndex(r => new { userId = r.UserId, activityId = r.ActivityId })
            .IsUnique();

        // Define index on (user_id, time_period_id)
        builder.HasIndex(r => new { userId = r.UserId, timePeriodId = r.TimePeriodId });

        builder.HasOne(r => r.TimePeriod)
            .WithMany()
            .HasForeignKey(r => r.TimePeriodId)
            .OnDelete(DeleteBehavior.Restrict); // Adjust cascade behavior as needed
    }
}