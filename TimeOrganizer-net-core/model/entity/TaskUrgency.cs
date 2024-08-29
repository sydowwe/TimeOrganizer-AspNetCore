using TimeOrganizer_net_core.model.entity.abs;

namespace TimeOrganizer_net_core.model.entity;

using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TaskUrgency : AbstractEntityWithUser
{
    public string Text { get; set; }
    public string Color { get; set; }
    public int Priority { get; set; }

    public ICollection<ToDoList> ToDoListItems { get; set; } = new List<ToDoList>();

    public TaskUrgency(long userId, string text, string color, int priority) : base(userId)
    {
        this.Text = text;
        this.Color = color;
        this.Priority = priority;
    }
}
public class TaskUrgencyConfiguration : IEntityTypeConfiguration<TaskUrgency>
{
    public void Configure(EntityTypeBuilder<TaskUrgency> builder)
    {
        builder.ToTable("TaskUrgency", schema: "public");

        // Define unique constraints
        builder.HasIndex(t => new { userId = t.UserId, text = t.Text })
            .IsUnique();

        builder.HasIndex(t => new { userId = t.UserId, priority = t.Priority })
            .IsUnique();

        // Configure the relationship
        builder.HasMany(t => t.ToDoListItems)
            .WithOne(tl => tl.TaskUrgency)
            .HasForeignKey(tl => tl.TaskUrgencyId)
            .OnDelete(DeleteBehavior.Restrict); // Adjust cascade behavior as needed
    }
}