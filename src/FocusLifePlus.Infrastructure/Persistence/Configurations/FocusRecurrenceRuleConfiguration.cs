using FocusLifePlus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;

namespace FocusLifePlus.Infrastructure.Persistence.Configurations;

public class FocusRecurrenceRuleConfiguration : IEntityTypeConfiguration<FocusRecurrenceRule>
{
    public void Configure(EntityTypeBuilder<FocusRecurrenceRule> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Type)
            .IsRequired();

        builder.Property(r => r.Interval)
            .IsRequired();

        builder.Property(r => r.StartDate)
            .IsRequired();

        builder.Property(r => r.WeekDays)
            .HasConversion(
                v => string.Join(",", v),
                v => v.Split(",", StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Enum.Parse<DayOfWeek>(x))
                    .ToArray())
            .Metadata.SetValueComparer(
                new ValueComparer<DayOfWeek[]>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToArray()));

        builder.Property(r => r.MonthDays)
            .HasConversion(
                v => string.Join(",", v),
                v => v.Split(",", StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray())
            .Metadata.SetValueComparer(
                new ValueComparer<int[]>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v)),
                    c => c.ToArray()));

        builder.HasOne(r => r.Task)
            .WithOne(t => t.RecurrenceRule)
            .HasForeignKey<FocusRecurrenceRule>(r => r.TaskId)
            .OnDelete(DeleteBehavior.NoAction);
    }
} 