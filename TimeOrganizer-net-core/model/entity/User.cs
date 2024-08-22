using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TimeOrganizer_net_core.model.entity.abs;

namespace TimeOrganizer_net_core.model.entity;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public enum AvailableLocales
{
    En,
    Sk,
    // add more locales as needed
}

public class User : IdentityUser<long>
{
    // Name
    [Required] public string name { get; set; }

    // Surname
    [Required] public string surname { get; set; }

    // Stay Logged In
    [Required] public bool isStayLoggedIn { get; set; }

    // Current Locale
    [Required] public AvailableLocales currentLocale { get; set; } = AvailableLocales.Sk;

    // Timezone
    [Required] public TimeZoneInfo timezone { get; set; } // Assuming ZoneIdDBConverter is for ZoneId, store as string
    
    // Navigation properties for related entities
    public virtual ICollection<Activity> activityList { get; set; } = new List<Activity>();
    public virtual ICollection<Category> categoryList { get; set; } = new List<Category>();
    public virtual ICollection<History> historyList { get; set; } = new List<History>();
    public virtual ICollection<Role> roleList { get; set; } = new List<Role>();
    public virtual ICollection<ToDoList> toDoListList { get; set; } = new List<ToDoList>();
    public virtual ICollection<TaskUrgency> taskUrgencyList { get; set; } = new List<TaskUrgency>();

    public User() : base()
    {
    }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User", schema: "public");

        // Primary key
        builder.HasKey(u => u.Id);

        // Indexes
        builder.HasIndex(u => u.Email).IsUnique(); // Unique constraint on Email

        // Property configurations
        builder.Property(u => u.name)
            .IsRequired()
            .HasMaxLength(100); // Adjust length as needed

        builder.Property(u => u.surname)
            .IsRequired()
            .HasMaxLength(100); // Adjust length as needed

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256); // Adjust length as needed

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        builder.Property(u => u.isStayLoggedIn)
            .IsRequired();



        builder.Property(u => u.currentLocale)
            .HasConversion(
                v => v.ToString(),
                v => (AvailableLocales)Enum.Parse(typeof(AvailableLocales), v))
            .IsRequired();
        // Time zone conversion
        builder.Property(u => u.timezone)
            .HasConversion(tz => tz.Id,
                id => TimeZoneInfo.FindSystemTimeZoneById(id));
        
        // Relationships
        builder.HasMany(u => u.activityList)
            .WithOne(a => a.user)
            .HasForeignKey(a => a.userId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.categoryList)
            .WithOne(c => c.user)
            .HasForeignKey(c => c.userId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.historyList)
            .WithOne(h => h.user)
            .HasForeignKey(h => h.userId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.roleList)
            .WithOne(r => r.user)
            .HasForeignKey(r => r.userId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.toDoListList)
            .WithOne(t => t.user)
            .HasForeignKey(t => t.userId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.taskUrgencyList)
            .WithOne(t => t.user)
            .HasForeignKey(t => t.userId)
            .OnDelete(DeleteBehavior.Cascade);

        
    }
}