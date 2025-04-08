using FocusLifePlus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FocusLifePlus.Infrastructure.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.NormalizedName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Description)
            .HasMaxLength(200);

        builder.Property(x => x.Permissions)
            .HasMaxLength(1000);

        builder.HasIndex(x => x.NormalizedName);

        // Seed data
        var seedDate = new DateTime(2024, 1, 1);
        builder.HasData(
            new Role
            {
                Id = Guid.Parse("8D04DCE2-969A-435D-BBA4-DF3F325983DC"),
                Name = "Admin",
                NormalizedName = "ADMIN",
                Description = "Administrator role with full access",
                CreatedAt = seedDate,
                CreatedBy = Guid.Parse("00000000-0000-0000-0000-000000000001")
            },
            new Role
            {
                Id = Guid.Parse("8D04DCE2-969A-435D-BBA4-DF3F325983DD"),
                Name = "User",
                NormalizedName = "USER",
                Description = "Standard user role",
                CreatedAt = seedDate,
                CreatedBy = Guid.Parse("00000000-0000-0000-0000-000000000001")
            }
        );
    }
} 