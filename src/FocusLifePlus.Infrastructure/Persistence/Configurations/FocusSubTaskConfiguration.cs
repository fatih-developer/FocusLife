using FocusLifePlus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FocusLifePlus.Infrastructure.Persistence.Configurations;

public class FocusSubTaskConfiguration : IEntityTypeConfiguration<FocusSubTask>
{
    public void Configure(EntityTypeBuilder<FocusSubTask> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.Description)
            .HasMaxLength(1000);

        builder.Property(s => s.Order)
            .IsRequired();

        builder.Property(s => s.Status)
            .IsRequired();

        builder.Property(s => s.ProgressPercentage)
            .IsRequired();

        builder.HasOne(s => s.ParentTask)
            .WithMany(t => t.SubTasks)
            .HasForeignKey(s => s.ParentTaskId)
            .OnDelete(DeleteBehavior.NoAction);
    }
} 