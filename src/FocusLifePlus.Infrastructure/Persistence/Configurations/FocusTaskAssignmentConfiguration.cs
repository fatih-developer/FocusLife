using FocusLifePlus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FocusLifePlus.Infrastructure.Persistence.Configurations;

public class FocusTaskAssignmentConfiguration : IEntityTypeConfiguration<FocusTaskAssignment>
{
    public void Configure(EntityTypeBuilder<FocusTaskAssignment> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Role)
            .IsRequired();

        builder.Property(a => a.AssignedAt)
            .IsRequired();

        builder.HasOne(a => a.FocusTask)
            .WithMany(t => t.Assignments)
            .HasForeignKey(a => a.FocusTaskId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.AssignedToUser)
            .WithMany()
            .HasForeignKey(a => a.AssignedToUserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(a => a.AssignedByUser)
            .WithMany()
            .HasForeignKey(a => a.AssignedByUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
} 