using TimeOrganizer_net_core.model.entity.abs;

namespace TimeOrganizer_net_core.model.entity;

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RoutineTimePeriod : AbstractEntityWithUser
{
    public string Text { get; set; }
    public string Color { get; set; }
    public int LengthInDays { get; set; }
    public bool IsHiddenInView { get; set; }
    public ICollection<RoutineToDoList> ToDoListItems { get; set; } = new List<RoutineToDoList>();

    public RoutineTimePeriod(long userId, string text, string color, int lengthInDays, bool isHiddenInView) : base(userId)
    {
        this.Text = text;
        this.Color = color;
        this.LengthInDays = lengthInDays;
        this.IsHiddenInView = isHiddenInView;
    }
}
public class RoutineTimePeriodConfiguration : IEntityTypeConfiguration<RoutineTimePeriod>
{
    public void Configure(EntityTypeBuilder<RoutineTimePeriod> builder)
    {
        builder.ToTable("RoutineTimePeriod", schema: "public");

        // Define unique constraint on (user_id, text)
        builder.HasIndex(r => new { userId = r.UserId, text = r.Text })
            .IsUnique();
            
        // Configure the relationship
        builder.HasMany(r => r.ToDoListItems)
            .WithOne(t => t.TimePeriod)
            .HasForeignKey(t => t.TimePeriodId)
            .OnDelete(DeleteBehavior.Restrict); // Adjust cascade behavior as needed
    }
}