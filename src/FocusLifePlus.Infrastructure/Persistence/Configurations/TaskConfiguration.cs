using FocusLifePlus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FocusLifePlus.Infrastructure.Persistence.Configurations;

public class TaskConfiguration : IEntityTypeConfiguration<FocusTask>
{
    public void Configure(EntityTypeBuilder<FocusTask> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Description)
            .IsRequired();

        builder.Property(t => t.DueDate)
            .IsRequired();

        builder.Property(t => t.Priority)
            .IsRequired();

        builder.Property(t => t.Status)
            .IsRequired();

        builder.Property(t => t.ProgressPercentage)
            .IsRequired()
            .HasDefaultValue(0);

        builder.HasOne(t => t.User)
            .WithMany(u => u.Tasks)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(t => t.FocusCategory)
            .WithMany(c => c.Tasks)
            .HasForeignKey(t => t.FocusCategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(t => t.Tags)
            .WithMany(t => t.Tasks)
            .UsingEntity(j => j.ToTable("TaskTagMappings"));

        builder.HasMany(t => t.SubTasks)
            .WithOne(st => st.ParentTask)
            .HasForeignKey(st => st.ParentTaskId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.Reminders)
            .WithOne(r => r.Task)
            .HasForeignKey(r => r.TaskId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.History)
            .WithOne(h => h.FocusTask)
            .HasForeignKey(h => h.FocusTaskId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.Comments)
            .WithOne(c => c.Task)
            .HasForeignKey(c => c.TaskId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.Assignments)
            .WithOne(a => a.FocusTask)
            .HasForeignKey(a => a.FocusTaskId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Tasks");
    }
} 