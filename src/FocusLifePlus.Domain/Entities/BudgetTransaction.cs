using FocusLifePlus.Domain.Common;
using FocusLifePlus.Domain.Enums;

namespace FocusLifePlus.Domain.Entities;

public class FocusBudgetTransaction : BaseEntity
{
    public decimal Amount { get; set; }
    public string Description { get; set; } = null!;
    public DateTime TransactionDate { get; set; }
    public TransactionType Type { get; set; }
    public Guid BudgetId { get; set; }
    public virtual FocusBudget Budget { get; set; } = null!;
} 