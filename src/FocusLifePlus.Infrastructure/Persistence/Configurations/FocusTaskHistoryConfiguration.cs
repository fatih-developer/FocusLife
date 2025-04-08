using FocusLifePlus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FocusLifePlus.Infrastructure.Persistence.Configurations;

public class FocusTaskHistoryConfiguration : IEntityTypeConfiguration<FocusTaskHistory>
{
    public void Configure(EntityTypeBuilder<FocusTaskHistory> builder)
    {
        builder.HasKey(h => h.Id);

        builder.Property(h => h.FieldName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(h => h.OldValue)
            .HasMaxLength(500);

        builder.Property(h => h.NewValue)
            .HasMaxLength(500);

        builder.Property(h => h.ChangedAt)
            .IsRequired();

        builder.HasOne(h => h.FocusTask)
            .WithMany(t => t.History)
            .HasForeignKey(h => h.FocusTaskId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(h => h.ChangedByUser)
            .WithMany()
            .HasForeignKey(h => h.ChangedByUserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
} 