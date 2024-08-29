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
    public DateTime StartTimestamp { get; set; }

    [Required]
    [Column(TypeName = "int")]
    public MyIntTime Length { get; set; }

    [Required]
    public DateTime EndTimestamp { get; set; }
}
public class ActivityHistoryConfiguration : IEntityTypeConfiguration<ActivityHistory>
{
    public void Configure(EntityTypeBuilder<ActivityHistory> builder)
    {
        builder.ToTable("ActivityHistory", schema: "public");
        builder.HasIndex(h => h.StartTimestamp);

        builder.Property(h => h.Length)
            .HasConversion(new MyIntTimeConverter());
    }
}
