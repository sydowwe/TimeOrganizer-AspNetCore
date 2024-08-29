using TimeOrganizer_net_core.model.entity.abs;

namespace TimeOrganizer_net_core.model.entity;

using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Category : AbstractAbstractNameTextColorEntity
{
    public virtual ICollection<Activity> activities { get; set; } = new List<Activity>();

    // public string icon { get; set; }
}
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Category", schema: "public");

        builder.HasIndex(r => new { r.userId, r.name })
            .IsUnique();
    }
}