using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.model.entity.abs;

namespace TimeOrganizer_net_core;

using Microsoft.EntityFrameworkCore;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> users { get; set; }
    public DbSet<Activity> activities { get; set; }
    public DbSet<Alarm> alarms { get; set; }
    public DbSet<Category> categories { get; set; }
    public DbSet<Role> roles { get; set; }
    public DbSet<History> histories { get; set; }
    public DbSet<PlannerTask> plannerTasks { get; set; }
    public DbSet<RoutineToDoList> routineToDoLists { get; set; }
    public DbSet<RoutineTimePeriod> routineTimePeriods { get; set; }
    public DbSet<ToDoList> toDoLists { get; set; }
    public DbSet<TaskUrgency> taskUrgencies { get; set; }
    public DbSet<WebExtensionData> webExtensionData { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AbstractEntityWithActivity>().UseTpcMappingStrategy();

        modelBuilder.ApplyConfiguration(new AlarmConfiguration());
        modelBuilder.ApplyConfiguration(new ActivityConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new HistoryConfiguration());
        modelBuilder.ApplyConfiguration(new PlannerTaskConfiguration());
        modelBuilder.ApplyConfiguration(new RoutineToDoListConfiguration());
        modelBuilder.ApplyConfiguration(new ToDoListConfiguration());
        modelBuilder.ApplyConfiguration(new RoutineTimePeriodConfiguration());
        modelBuilder.ApplyConfiguration(new TaskUrgencyConfiguration());
        modelBuilder.ApplyConfiguration(new WebExtensionDataConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        base.OnModelCreating(modelBuilder);
    }
    
    
    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries<AbstractEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.createdTimestamp = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.modifiedTimestamp = DateTime.UtcNow;
            }
        }

        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<AbstractEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.createdTimestamp = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.modifiedTimestamp = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
    private void ConfigureNameTextEntity<TEntity>(EntityTypeBuilder<TEntity> builder) 
        where TEntity : AbstractNameTextEntity
    {
        builder.HasIndex(e => new { e.userId, e.name })
            .IsUnique()
            .HasDatabaseName($"{typeof(TEntity).Name.ToLower()}_unique_user_id_name");
    }
}
