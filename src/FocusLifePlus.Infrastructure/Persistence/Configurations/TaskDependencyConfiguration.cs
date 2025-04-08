using FocusLifePlus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FocusLifePlus.Infrastructure.Persistence.Configurations;

public class TaskDependencyConfiguration : IEntityTypeConfiguration<FocusTaskDependency>
{
    public void Configure(EntityTypeBuilder<FocusTaskDependency> builder)
    {
        builder.HasKey(td => td.Id);

        builder.Property(td => td.Type)
            .IsRequired();

        builder.HasOne(td => td.DependentTask)
            .WithMany(t => t.Dependencies)
            .HasForeignKey(td => td.DependentTaskId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(td => td.DependencyTask)
            .WithMany(t => t.Dependents)
            .HasForeignKey(td => td.DependencyTaskId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.ToTable("TaskDependencies");
    }
} 