using System.ComponentModel.DataAnnotations;
using TimeOrganizer_net_core.model.entity.abs;

namespace TimeOrganizer_net_core.model.entity;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PlannerTask : AbstractEntityWithIsDone
{
    public DateTime startTimestamp { get; set; }
    [Required]
    public int minuteLength { get; set; }
    public string color { get; set; }
}
public class PlannerTaskConfiguration : IEntityTypeConfiguration<PlannerTask>
{
    public void Configure(EntityTypeBuilder<PlannerTask> builder)
    {
        builder.ToTable("PlannerTask", schema: "public");

        builder.HasIndex(p => new { p.userId, p.startTimestamp })
            .IsUnique();

        // Configure the relationship
        builder.HasOne(p => p.activity)
            .WithMany()
            .HasForeignKey(p => p.activityId)
            .OnDelete(DeleteBehavior.Restrict); // Adjust cascade behavior as needed
    }
}
