using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeOrganizer_net_core.model.entity.abs;

namespace TimeOrganizer_net_core.model.entity;

public class Alarm : AbstractEntityWithActivity
{
    public DateTime StartTimestamp { get; set; }
    public bool IsActive { get; set; }

    public Alarm()
    {
    }
}
public class AlarmConfiguration : IEntityTypeConfiguration<Alarm>
{
    public void Configure(EntityTypeBuilder<Alarm> builder)
    {
        builder.ToTable("Alarm", schema: "public");

        // Define indexing if needed
        builder.HasIndex(h => h.StartTimestamp);
    }
}