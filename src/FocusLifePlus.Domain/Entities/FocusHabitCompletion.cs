using FocusLifePlus.Domain.Common;

namespace FocusLifePlus.Domain.Entities;

public class FocusHabitCompletion : BaseEntity
{
    public Guid HabitId { get; set; }
    public virtual FocusHabit Habit { get; set; } = null!;
    public DateTime CompletedAt { get; set; }
    public string? Notes { get; set; }
} 