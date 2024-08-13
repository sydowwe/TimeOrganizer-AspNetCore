using System.ComponentModel.DataAnnotations.Schema;
using TimeOrganizer_net_core.model.entity.abs;

namespace TimeOrganizer_net_core.model.entity;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ToDoList : AbstractEntityWithIsDone
{
    [ForeignKey("taskUrgencyId")]
    public virtual TaskUrgency taskUrgency { get; set; }
    public long taskUrgencyId { get; set; }
}
public class ToDoListConfiguration : IEntityTypeConfiguration<ToDoList>
{
    public void Configure(EntityTypeBuilder<ToDoList> builder)
    {
        builder.ToTable("ToDoList", schema: "public");

        // Define unique constraint on (user_id, activity_id)
        builder.HasIndex(t => new { t.userId, t.activityId })
            .IsUnique();
        
        // Define index on (user_id, task_urgency_id)
        builder.HasIndex(t => new { t.userId, t.taskUrgencyId });

        builder.HasOne(t => t.taskUrgency)
            .WithMany()
            .HasForeignKey(t => t.taskUrgencyId)
            .OnDelete(DeleteBehavior.Restrict); // Adjust cascade behavior as needed
    }
}