using FocusLifePlus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FocusLifePlus.Infrastructure.Persistence.Configurations;

public class FocusTaskDependencyConfiguration : IEntityTypeConfiguration<FocusTaskDependency>
{
    public void Configure(EntityTypeBuilder<FocusTaskDependency> builder)
    {
        builder.HasKey(d => d.Id);

        builder.HasOne(d => d.DependentTask)
            .WithMany(t => t.Dependencies)
            .HasForeignKey(d => d.DependentTaskId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(d => d.DependencyTask)
            .WithMany(t => t.Dependents)
            .HasForeignKey(d => d.DependencyTaskId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(d => d.Type)
            .IsRequired();
    }
} 