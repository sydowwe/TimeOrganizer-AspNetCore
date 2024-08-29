using System.ComponentModel.DataAnnotations.Schema;
using TimeOrganizer_net_core.model.entity.abs;

namespace TimeOrganizer_net_core.model.entity;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ToDoList : AbstractEntityWithIsDone
{
    [ForeignKey("taskUrgencyId")]
    public virtual TaskUrgency TaskUrgency { get; set; }
    public long TaskUrgencyId { get; set; }
}
public class ToDoListConfiguration : IEntityTypeConfiguration<ToDoList>
{
    public void Configure(EntityTypeBuilder<ToDoList> builder)
    {
        builder.ToTable("ToDoList", schema: "public");

        // Define unique constraint on (user_id, activity_id)
        builder.HasIndex(t => new { userId = t.UserId, activityId = t.ActivityId })
            .IsUnique();
        
        // Define index on (user_id, task_urgency_id)
        builder.HasIndex(t => new { userId = t.UserId, taskUrgencyId = t.TaskUrgencyId });

        builder.HasOne(t => t.TaskUrgency)
            .WithMany()
            .HasForeignKey(t => t.TaskUrgencyId)
            .OnDelete(DeleteBehavior.Restrict); // Adjust cascade behavior as needed
    }
}