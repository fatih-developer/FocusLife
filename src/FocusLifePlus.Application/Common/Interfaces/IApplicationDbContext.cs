using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FocusLifePlus.Domain.Entities;

namespace FocusLifePlus.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<FocusTask> FocusTasks { get; }
        DbSet<FocusCategory> Categories { get; }
        DbSet<FocusNote> Notes { get; }
        DbSet<FocusTag> Tags { get; }
        DbSet<FocusHabit> Habits { get; }
        DbSet<FocusHabitCompletion> HabitCompletions { get; }
        DbSet<FocusGoal> Goals { get; }
        DbSet<FocusGoalMilestone> GoalMilestones { get; }
        DbSet<FocusBudget> Budgets { get; }
        DbSet<FocusBudgetTransaction> BudgetTransactions { get; }
        DbSet<User> Users { get; }
        DbSet<Role> Roles { get; }
        DbSet<UserRole> UserRoles { get; }
        DbSet<FocusSubTask> SubTasks { get; }
        DbSet<FocusReminder> Reminders { get; }
        DbSet<FocusRecurrenceRule> RecurrenceRules { get; }
        DbSet<FocusTaskDependency> TaskDependencies { get; }
        DbSet<FocusTaskHistory> TaskHistories { get; }
        DbSet<FocusTaskComment> TaskComments { get; }
        DbSet<FocusTaskAssignment> TaskAssignments { get; }
        DbSet<RefreshToken> RefreshTokens { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
} 