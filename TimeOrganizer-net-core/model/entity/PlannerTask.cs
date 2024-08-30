using System.ComponentModel.DataAnnotations;
using TimeOrganizer_net_core.model.entity.abs;

namespace TimeOrganizer_net_core.model.entity;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PlannerTask : AbstractEntityWithIsDone
{
    public DateTime StartTimestamp { get; set; }
    [Required]
    public int MinuteLength { get; set; }
    [StringLength(6)]
    public string Color { get; set; } = "#1A237E";
}
public class PlannerTaskConfiguration : IEntityTypeConfiguration<PlannerTask>
{
    public void Configure(EntityTypeBuilder<PlannerTask> builder)
    {
        builder.ToTable("PlannerTask", schema: "public");

        builder.HasIndex(p => new { userId = p.UserId, startTimestamp = p.StartTimestamp })
            .IsUnique();
    }
}
