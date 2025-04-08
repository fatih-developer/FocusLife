using FocusLifePlus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FocusLifePlus.Infrastructure.Persistence.Configurations;

public class HabitCompletionConfiguration : IEntityTypeConfiguration<FocusHabitCompletion>
{
    public void Configure(EntityTypeBuilder<FocusHabitCompletion> builder)
    {
        builder.ToTable("HabitCompletions");

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Habit)
            .WithMany(x => x.Completions)
            .HasForeignKey(x => x.HabitId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.CompletedAt)
            .IsRequired();

        builder.Property(x => x.Notes)
            .HasMaxLength(500);
    }
} 