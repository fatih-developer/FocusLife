using FocusLifePlus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FocusLifePlus.Infrastructure.Persistence.Configurations;

public class FocusReminderConfiguration : IEntityTypeConfiguration<FocusReminder>
{
    public void Configure(EntityTypeBuilder<FocusReminder> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.ReminderTime)
            .IsRequired();

        builder.Property(r => r.Message)
            .HasMaxLength(500);

        builder.HasOne(r => r.Task)
            .WithMany(t => t.Reminders)
            .HasForeignKey(r => r.TaskId)
            .OnDelete(DeleteBehavior.NoAction);
    }
} 