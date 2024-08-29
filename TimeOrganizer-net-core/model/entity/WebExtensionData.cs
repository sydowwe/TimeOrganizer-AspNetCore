using TimeOrganizer_net_core.model.entity.abs;

namespace TimeOrganizer_net_core.model.entity;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class WebExtensionData : AbstractEntityWithActivity
{
    public string Domain { get; set; }
    public string Title { get; set; }
    public int Duration { get; set; }
    public DateTime StartTimestamp { get; set; }
}
public class WebExtensionDataConfiguration : IEntityTypeConfiguration<WebExtensionData>
{
    public void Configure(EntityTypeBuilder<WebExtensionData> builder)
    {
        builder.ToTable("WebExtensionData", schema: "public");

        // Define unique constraint on (user_id, domain)
        builder.HasIndex(w => new { userId = w.UserId, domain = w.Domain })
            .IsUnique();
    }
}