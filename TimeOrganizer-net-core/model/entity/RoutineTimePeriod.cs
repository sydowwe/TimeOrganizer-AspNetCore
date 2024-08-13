using TimeOrganizer_net_core.model.entity.abs;

namespace TimeOrganizer_net_core.model.entity;

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RoutineTimePeriod : AbstractEntityWithUser
{
    public string text { get; set; }
    public string color { get; set; }
    public int lengthInDays { get; set; }
    public bool isHiddenInView { get; set; }
    public ICollection<RoutineToDoList> toDoListItems { get; set; } = new List<RoutineToDoList>();

    public RoutineTimePeriod(long userId, string text, string color, int lengthInDays, bool isHiddenInView) : base(userId)
    {
        this.text = text;
        this.color = color;
        this.lengthInDays = lengthInDays;
        this.isHiddenInView = isHiddenInView;
    }
}
public class RoutineTimePeriodConfiguration : IEntityTypeConfiguration<RoutineTimePeriod>
{
    public void Configure(EntityTypeBuilder<RoutineTimePeriod> builder)
    {
        builder.ToTable("RoutineTimePeriod", schema: "public");

        // Define unique constraint on (user_id, text)
        builder.HasIndex(r => new { r.userId, r.text })
            .IsUnique();
            
        // Configure the relationship
        builder.HasMany(r => r.toDoListItems)
            .WithOne(t => t.timePeriod)
            .HasForeignKey(t => t.timePeriodId)
            .OnDelete(DeleteBehavior.Restrict); // Adjust cascade behavior as needed
    }
}