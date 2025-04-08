using System;
using System.Collections.Generic;
using FocusLifePlus.Domain.Common;
using FocusLifePlus.Domain.Enums;

namespace FocusLifePlus.Domain.Entities
{
    public class FocusGoal : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public GoalStatus Status { get; set; }
        public GoalPriority Priority { get; set; }
        public int ProgressPercentage { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; } = null!;
        public Guid? FocusCategoryId { get; set; }
        public FocusCategory FocusCategory { get; set; }
        public ICollection<GoalMilestone> Milestones { get; set; } = new List<GoalMilestone>();
        public DateTime? CompletedAt { get; set; }
    }

    public class GoalMilestone : BaseEntity
    {
        public Guid GoalId { get; set; }
        public FocusGoal Goal { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
    }

    public enum GoalStatus
    {
        NotStarted,
        InProgress,
        Completed,
        Cancelled
    }

    public enum GoalPriority
    {
        Low,
        Medium,
        High,
        Critical
    }
} 