using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TimeOrganizer_net_core.model.entity.abs;

namespace TimeOrganizer_net_core.model.entity;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public enum UserRole
{
    Admin,
    User,
    Guest
}

public enum AvailableLocales
{
    En,
    Sk,
    // add more locales as needed
}

public class User : AbstractEntity
{
    // Name
    [Required] public string name { get; set; }

    // Surname
    [Required] public string surname { get; set; }

    // Email
    [Required]
    [EmailAddress]
    //Unique
    public string email { get; set; }

    // Password
    [Required] public string password { get; set; }

    // 2FA Secret Key
    //Unique
    public string secretKey2FA { get; set; }

    // User Role
    [Required] public UserRole role { get; set; }

    // Scratch Codes
    public List<int> scratchCodes { get; set; } = new List<int>();

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

    public User(string name, string surname, string email, string password, UserRole role, AvailableLocales locale,
        TimeZoneInfo timezone) : base()
    {
        this.name = name;
        this.surname = surname;
        this.email = email;
        this.password = password;
        this.role = role;
        this.currentLocale = locale;
        this.timezone = timezone;
    }

    // Method to check if 2FA is enabled
    public bool has2FA()
    {
        return !string.IsNullOrWhiteSpace(secretKey2FA);
    }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User", schema: "public");

        // Primary key
        builder.HasKey(u => u.id);

        // Indexes
        builder.HasIndex(u => u.email).IsUnique(); // Unique constraint on Email

        // Property configurations
        builder.Property(u => u.name)
            .IsRequired()
            .HasMaxLength(100); // Adjust length as needed

        builder.Property(u => u.surname)
            .IsRequired()
            .HasMaxLength(100); // Adjust length as needed

        builder.Property(u => u.email)
            .IsRequired()
            .HasMaxLength(256); // Adjust length as needed

        builder.Property(u => u.password)
            .IsRequired();

        builder.Property(u => u.secretKey2FA)
            .HasMaxLength(256); // Adjust length as needed

        builder.Property(u => u.isStayLoggedIn)
            .IsRequired();

        // Enum configuration
        builder.Property(u => u.role)
            .HasConversion(
                v => v.ToString(),
                v => (UserRole)Enum.Parse(typeof(UserRole), v))
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