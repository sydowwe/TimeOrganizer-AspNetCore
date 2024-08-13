using TimeOrganizer_net_core.model.entity.abs;

namespace TimeOrganizer_net_core.model.entity;

using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TaskUrgency : AbstractEntityWithUser
{
    public string text { get; set; }
    public string color { get; set; }
    public int priority { get; set; }

    public ICollection<ToDoList> toDoListItems { get; set; } = new List<ToDoList>();

    public TaskUrgency(long userId, string text, string color, int priority) : base(userId)
    {
        this.text = text;
        this.color = color;
        this.priority = priority;
    }
}
public class TaskUrgencyConfiguration : IEntityTypeConfiguration<TaskUrgency>
{
    public void Configure(EntityTypeBuilder<TaskUrgency> builder)
    {
        builder.ToTable("TaskUrgency", schema: "public");

        // Define unique constraints
        builder.HasIndex(t => new { t.userId, t.text })
            .IsUnique();

        builder.HasIndex(t => new { t.userId, t.priority })
            .IsUnique();

        // Configure the relationship
        builder.HasMany(t => t.toDoListItems)
            .WithOne(tl => tl.taskUrgency)
            .HasForeignKey(tl => tl.taskUrgencyId)
            .OnDelete(DeleteBehavior.Restrict); // Adjust cascade behavior as needed
    }
}