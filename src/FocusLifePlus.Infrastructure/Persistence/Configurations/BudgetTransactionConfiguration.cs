using FocusLifePlus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FocusLifePlus.Infrastructure.Persistence.Configurations;

public class FocusBudgetTransactionConfiguration : IEntityTypeConfiguration<BudgetTransaction>
{
    public void Configure(EntityTypeBuilder<BudgetTransaction> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.TransactionDate)
            .IsRequired();

        builder.Property(x => x.Type)
            .IsRequired();

        builder.HasOne(x => x.Budget)
            .WithMany(b => b.Transactions)
            .HasForeignKey(x => x.BudgetId)
            .OnDelete(DeleteBehavior.NoAction);
    }
} 