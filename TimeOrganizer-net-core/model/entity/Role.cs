using TimeOrganizer_net_core.model.entity.abs;

namespace TimeOrganizer_net_core.model.entity;

using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Role : AbstractAbstractNameTextColorEntity
{
    // public string icon { get; set; }
    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();

    public Role() : base() {}

    public Role(long userId, string name, string text, string color, string icon) : base(name, text, color, userId)
    {
        // this.icon = icon;
    }
}
public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Role", schema: "public");

        builder.HasIndex(r => new { userId = r.UserId, name = r.Name })
            .IsUnique();
    }
}