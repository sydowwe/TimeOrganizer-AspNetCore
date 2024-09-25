using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TimeOrganizer_net_core.model.entity;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public enum AvailableLocales
{
    En,
    Sk,
    Cz
}

public class User : IdentityUser<long>
{
    // Name
    [Required] public string Name { get; set; }

    // Surname
    [Required] public string Surname { get; set; }

    // TODO REMOVE
    [Required] public bool IsStayLoggedIn { get; set; }

    // Current Locale
    [Required] public AvailableLocales CurrentLocale { get; set; } = AvailableLocales.Sk;

    // Timezone
    [Required] public TimeZoneInfo Timezone { get; set; } // Assuming ZoneIdDBConverter is for ZoneId, store as string
    
    // Navigation properties for related entities
    public virtual ICollection<Activity> ActivityList { get; set; } = new List<Activity>();
    public virtual ICollection<Category> CategoryList { get; set; } = new List<Category>();
    public virtual ICollection<ActivityHistory> ActivityHistoryList { get; set; } = new List<ActivityHistory>();
    public virtual ICollection<Role> RoleList { get; set; } = new List<Role>();
    public virtual ICollection<ToDoList> ToDoListList { get; set; } = new List<ToDoList>();
    public virtual ICollection<TaskUrgency> TaskUrgencyList { get; set; } = new List<TaskUrgency>();
    
    
    // [NotMapped]
    // public override string? UserName
    // {
    //     get => Email;
    //     set { } 
    // }
    //
    // [NotMapped]
    // public override string? NormalizedUserName
    // {
    //     get => NormalizedEmail;
    //     set { } 
    // }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {

        // Primary key
        builder.HasKey(u => u.Id);

        // Indexes
        builder.HasIndex(u => u.Email).IsUnique(); // Unique constraint on Email

        // Property configurations
        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100); // Adjust length as needed

        builder.Property(u => u.Surname)
            .IsRequired()
            .HasMaxLength(100); // Adjust length as needed

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256); // Adjust length as needed

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        builder.Property(u => u.IsStayLoggedIn)
            .IsRequired();



        builder.Property(u => u.CurrentLocale)
            .HasConversion(
                v => v.ToString(),
                v => (AvailableLocales)Enum.Parse(typeof(AvailableLocales), v))
            .IsRequired();
        // Time zone conversion
        builder.Property(u => u.Timezone)
            .HasConversion(tz => tz.Id,
                id => TimeZoneInfo.FindSystemTimeZoneById(id));
        
        // Relationships
        builder.HasMany(u => u.ActivityList)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.CategoryList)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.ActivityHistoryList)
            .WithOne(h => h.User)
            .HasForeignKey(h => h.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.RoleList)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.ToDoListList)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.TaskUrgencyList)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        
    }
}