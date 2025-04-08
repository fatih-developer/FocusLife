using System;
using System.Collections.Generic;
using FocusLifePlus.Domain.Common;
using FocusLifePlus.Domain.Enums;

namespace FocusLifePlus.Domain.Entities
{
    public class FocusBudget : BaseEntity
    {
        public string Name { get; set; } = null!;
        public decimal Amount { get; set; }
        public decimal SpentAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public BudgetType Type { get; set; }
        public BudgetPeriod Period { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; } = null!;
        public ICollection<BudgetTransaction> Transactions { get; set; } = new List<BudgetTransaction>();
    }

    public class BudgetTransaction : BaseEntity
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public TransactionType Type { get; set; }
        public Guid BudgetId { get; set; }
        public FocusBudget Budget { get; set; }
    }

    public enum BudgetType
    {
        Income,
        Expense,
        Savings
    }

    public enum BudgetPeriod
    {
        Daily,
        Weekly,
        Monthly,
        Yearly,
        Custom
    }

    public enum TransactionType
    {
        Income,
        Expense
    }
} 