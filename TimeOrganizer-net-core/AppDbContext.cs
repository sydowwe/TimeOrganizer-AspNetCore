using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.model.entity.abs;
using TimeOrganizer_net_core.security;

namespace TimeOrganizer_net_core;

using Microsoft.EntityFrameworkCore;

public class AppDbContext(DbContextOptions<AppDbContext> options, ILoggedUserService loggedUserService) : IdentityDbContext<User, UserRole, long>(options)
{
    public DbSet<Activity> Activities { get; set; }
    public DbSet<Alarm> Alarms { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<ActivityHistory> ActivityHistories { get; set; }
    public DbSet<PlannerTask> PlannerTasks { get; set; }
    public DbSet<RoutineToDoList> RoutineToDoLists { get; set; }
    public DbSet<RoutineTimePeriod> RoutineTimePeriods { get; set; }
    public DbSet<ToDoList> ToDoLists { get; set; }
    public DbSet<TaskUrgency> TaskUrgencies { get; set; }
    public DbSet<WebExtensionData> WebExtensionData { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AbstractEntityWithActivity>().UseTpcMappingStrategy();

        modelBuilder.ApplyConfiguration(new AlarmConfiguration());
        modelBuilder.ApplyConfiguration(new ActivityConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new ActivityHistoryConfiguration());
        modelBuilder.ApplyConfiguration(new PlannerTaskConfiguration());
        modelBuilder.ApplyConfiguration(new RoutineToDoListConfiguration());
        modelBuilder.ApplyConfiguration(new ToDoListConfiguration());
        modelBuilder.ApplyConfiguration(new RoutineTimePeriodConfiguration());
        modelBuilder.ApplyConfiguration(new TaskUrgencyConfiguration());
        modelBuilder.ApplyConfiguration(new WebExtensionDataConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        base.OnModelCreating(modelBuilder);
    }
    // public override int SaveChanges()
    // {
    //     foreach (var entry in ChangeTracker.Entries<AbstractEntity>())
    //     {
    //         if (entry.State == EntityState.Added)
    //         {
    //             entry.Entity.createdTimestamp = DateTime.UtcNow;
    //         }
    //
    //         if (entry.State == EntityState.Modified)
    //         {
    //             entry.Entity.modifiedTimestamp = DateTime.UtcNow;
    //         }
    //     }
    //     
    //     return base.SaveChanges();
    // }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<AbstractEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedTimestamp = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedTimestamp = DateTime.UtcNow;
                    break;
            }
        }
        
        long? userId = null;
        if (ChangeTracker.Entries<AbstractEntityWithUser>().Any(entry => entry.State == EntityState.Added))
        {
            if (loggedUserService.IsAuthenticated())
            {
                try
                {
                    userId = loggedUserService.GetLoggedUserId();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to get logged user ID: {ex.Message}");
                }
            }
        }
        if (userId.HasValue)
        {
            foreach (var entry in ChangeTracker.Entries<AbstractEntityWithUser>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.UserId = userId.Value;
                }
            }
        }
        return await base.SaveChangesAsync(cancellationToken);
    }
}
