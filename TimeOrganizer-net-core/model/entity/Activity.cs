using TimeOrganizer_net_core.model.entity.abs;

namespace TimeOrganizer_net_core.model.entity;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
public class Activity : AbstractNameTextEntity
{
    [Required]
    public bool IsOnToDoList { get; set; }

    [Required]
    public bool IsUnavoidable { get; set; }

    [Required]
    [ForeignKey(nameof(RoleId))]
    public virtual Role Role { get; set; }

    [Required]
    public long RoleId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public virtual Category Category { get; set; }

    public long? CategoryId { get; set; }

    public virtual ICollection<Alarm> alarmList { get; set; } = new List<Alarm>();
    public virtual ICollection<ActivityHistory> HistoryList { get; set; } = new List<ActivityHistory>();
    public virtual ICollection<WebExtensionData> WebExtensionDataList { get; set; } = new List<WebExtensionData>();
}
public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.ToTable("Activity", schema: "public");
        
        builder.HasMany(a => a.alarmList)
            .WithOne(a=>a.Activity)
            .HasForeignKey(a=>a.ActivityId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasMany(a => a.HistoryList)
            .WithOne(h=>h.Activity)
            .HasForeignKey(h=>h.ActivityId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasMany(a => a.WebExtensionDataList)
            .WithOne(w=>w.Activity)
            .HasForeignKey(w=>w.ActivityId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}