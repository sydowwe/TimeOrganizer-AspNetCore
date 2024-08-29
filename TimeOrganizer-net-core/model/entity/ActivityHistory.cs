using TimeOrganizer_net_core.helper;
using TimeOrganizer_net_core.model.entity.abs;

namespace TimeOrganizer_net_core.model.entity;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
public class ActivityHistory : AbstractEntityWithActivity
{
    [Required]
    public DateTime startTimestamp { get; set; }

    [Required]
    [Column(TypeName = "int")]
    public MyIntTime length { get; set; }

    [Required]
    public DateTime endTimestamp { get; set; }
}
public class HistoryConfiguration : IEntityTypeConfiguration<ActivityHistory>
{
    public void Configure(EntityTypeBuilder<ActivityHistory> builder)
    {
        builder.ToTable("History", schema: "public");

        // Define indexing if needed
        builder.HasIndex(h => h.startTimestamp);

        builder.Property(h => h.length)
            .HasConversion(new MyIntTimeConverter());
        
        builder.HasOne(a => a.activity)
            .WithMany() // Specify if Activity has a collection to be navigated
            .HasForeignKey(a => a.activityId)
            .OnDelete(DeleteBehavior.SetNull); // Specify cascade behavior if needed
    }
}
