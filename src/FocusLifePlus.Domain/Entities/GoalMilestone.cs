using FocusLifePlus.Domain.Common;

namespace FocusLifePlus.Domain.Entities;

public class FocusGoalMilestone : BaseEntity
{
    public Guid GoalId { get; set; }
    public virtual FocusGoal Goal { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
} 