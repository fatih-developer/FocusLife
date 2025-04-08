using FocusLifePlus.Application.Common.Interfaces;
using FocusLifePlus.Domain.Common;
using FocusLifePlus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace FocusLifePlus.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private static readonly Guid AdminRoleId = new Guid("308660dc-ae51-480f-824d-7dca6714c3e2");
    private static readonly Guid UserRoleId = new Guid("84eb3236-f687-4c2e-8b60-14d4ab8b5ef4");

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<FocusTask> FocusTasks { get; set; }
    public DbSet<FocusCategory> Categories { get; set; }
    public DbSet<FocusNote> Notes { get; set; }
    public DbSet<FocusTag> Tags { get; set; }
    public DbSet<FocusHabit> Habits { get; set; }
    public DbSet<FocusHabitCompletion> HabitCompletions { get; set; }
    public DbSet<FocusGoal> Goals { get; set; }
    public DbSet<FocusGoalMilestone> GoalMilestones { get; set; }
    public DbSet<FocusBudget> Budgets { get; set; }
    public DbSet<FocusBudgetTransaction> BudgetTransactions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<FocusSubTask> SubTasks { get; set; }
    public DbSet<FocusReminder> Reminders { get; set; }
    public DbSet<FocusRecurrenceRule> RecurrenceRules { get; set; }
    public DbSet<FocusTaskDependency> TaskDependencies { get; set; }
    public DbSet<FocusTaskHistory> TaskHistories { get; set; }
    public DbSet<FocusTaskComment> TaskComments { get; set; }
    public DbSet<FocusTaskAssignment> TaskAssignments { get; set; }
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Tüm entity'ler için soft delete filtresi ekliyoruz
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
                var falseConstant = Expression.Constant(false);
                var lambda = Expression.Lambda(Expression.Equal(property, falseConstant), parameter);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }

        // Varsayılan rolleri ekliyoruz
        modelBuilder.Entity<Role>().HasData(
            new Role 
            { 
                Id = AdminRoleId,
                Name = "Admin",
                NormalizedName = "ADMIN",
                Description = "Sistem yöneticisi",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Role 
            { 
                Id = UserRoleId,
                Name = "User",
                NormalizedName = "USER",
                Description = "Standart kullanıcı",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        // Tablo isimlerini Focus prefix'i olmadan ayarlama
        modelBuilder.Entity<FocusTask>().ToTable("Tasks");
        modelBuilder.Entity<FocusCategory>().ToTable("Categories");
        modelBuilder.Entity<FocusNote>().ToTable("Notes");
        modelBuilder.Entity<FocusTag>().ToTable("Tags");
        modelBuilder.Entity<FocusHabit>().ToTable("Habits");
        modelBuilder.Entity<FocusHabitCompletion>().ToTable("HabitCompletions");
        modelBuilder.Entity<FocusGoal>().ToTable("Goals");
        modelBuilder.Entity<FocusGoalMilestone>().ToTable("GoalMilestones");
        modelBuilder.Entity<FocusBudget>().ToTable("Budgets");
        modelBuilder.Entity<FocusBudgetTransaction>().ToTable("BudgetTransactions");
        modelBuilder.Entity<FocusSubTask>().ToTable("SubTasks");
        modelBuilder.Entity<FocusReminder>().ToTable("Reminders");
        modelBuilder.Entity<FocusRecurrenceRule>().ToTable("RecurrenceRules");
        modelBuilder.Entity<FocusTaskDependency>().ToTable("TaskDependencies");
        modelBuilder.Entity<FocusTaskHistory>().ToTable("TaskHistories");
        modelBuilder.Entity<FocusTaskComment>().ToTable("TaskComments");
        modelBuilder.Entity<FocusTaskAssignment>().ToTable("TaskAssignments");

        // Many-to-Many ilişkileri
        modelBuilder.Entity<FocusNote>()
            .HasMany(n => n.Tags)
            .WithMany(t => t.Notes)
            .UsingEntity(j => j.ToTable("NoteTagMappings"));

        modelBuilder.Entity<FocusTask>()
            .HasMany(t => t.Tags)
            .WithMany(t => t.Tasks)
            .UsingEntity(j => j.ToTable("TaskTagMappings"));

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
} 