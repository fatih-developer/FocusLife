using FocusLifePlus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FocusLifePlus.Infrastructure.Persistence.Configurations;

public class FocusTagConfiguration : IEntityTypeConfiguration<FocusTag>
{
    public void Configure(EntityTypeBuilder<FocusTag> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.Color)
            .HasMaxLength(7); // #RRGGBB formatı için

        builder.HasMany(t => t.Tasks)
            .WithMany(task => task.Tags)
            .UsingEntity(j => j.ToTable("FocusTaskTags"));
    }
} 