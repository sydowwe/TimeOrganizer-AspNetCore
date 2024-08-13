using TimeOrganizer_net_core.model.entity.abs;

namespace TimeOrganizer_net_core.model.entity;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class WebExtensionData : AbstractEntityWithActivity
{
    public string domain { get; set; }
    public string title { get; set; }
    public int duration { get; set; }
    public DateTime startTimestamp { get; set; }

 
}
public class WebExtensionDataConfiguration : IEntityTypeConfiguration<WebExtensionData>
{
    public void Configure(EntityTypeBuilder<WebExtensionData> builder)
    {
        builder.ToTable("WebExtensionData", schema: "public");

        // Define unique constraint on (user_id, domain)
        builder.HasIndex(w => new { w.userId, w.domain })
            .IsUnique();
    }
}