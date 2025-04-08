using FocusLifePlus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FocusLifePlus.Infrastructure.Persistence.Configurations;

public class FocusCategoryConfiguration : IEntityTypeConfiguration<FocusCategory>
{
    public void Configure(EntityTypeBuilder<FocusCategory> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Description)
            .HasMaxLength(500);

        builder.Property(c => c.Color)
            .HasMaxLength(7); // #RRGGBB formatı için

        builder.Property(c => c.UserId)
            .IsRequired();

        builder.HasOne(c => c.ParentFocusCategory)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(c => c.ParentFocusCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Tasks)
            .WithOne(t => t.FocusCategory)
            .HasForeignKey(t => t.FocusCategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(c => c.User)
            .WithMany(u => u.Categories)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
} 