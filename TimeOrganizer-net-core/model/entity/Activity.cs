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
    public bool isOnToDoList { get; set; }

    [Required]
    public bool isUnavoidable { get; set; }

    [Required]
    [ForeignKey(nameof(roleId))]
    public virtual Role role { get; set; }

    [Required]
    public long roleId { get; set; }

    [ForeignKey(nameof(categoryId))]
    public virtual Category category { get; set; }

    public long? categoryId { get; set; }

    // public virtual ICollection<Alarm> alarmList { get; set; } = new List<Alarm>();
    public virtual ICollection<ActivityHistory> historyList { get; set; } = new List<ActivityHistory>();
    public virtual ICollection<WebExtensionData> webExtensionDataList { get; set; } = new List<WebExtensionData>();
}
public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.ToTable("Activity", schema: "public");

        builder.HasOne(a => a.role)
            .WithMany()
            .HasForeignKey(a => a.roleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.category)
            .WithMany()
            .HasForeignKey(a => a.categoryId)
            .OnDelete(DeleteBehavior.SetNull);
        
        // builder.HasMany(a => a.alarmList)
        //     .WithOne(w=>w.activity)
        //     .HasForeignKey(w=>w.activityId)
        //     .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasMany(a => a.historyList)
            .WithOne(h=>h.activity)
            .HasForeignKey(h=>h.activityId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasMany(a => a.webExtensionDataList)
            .WithOne(w=>w.activity)
            .HasForeignKey(w=>w.activityId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}