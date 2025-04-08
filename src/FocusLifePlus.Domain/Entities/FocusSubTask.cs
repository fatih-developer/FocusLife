using System;
using FocusLifePlus.Domain.Common;
using FocusLifePlus.Domain.Common.Enums;

namespace FocusLifePlus.Domain.Entities;

public class FocusSubTask : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int Order { get; set; }
    public Guid ParentTaskId { get; set; }
    public FocusTask ParentTask { get; set; }
    public FocusTaskStatus Status { get; set; }
    public int ProgressPercentage { get; set; }

    public void Complete()
    {
        IsCompleted = true;
        CompletedAt = DateTime.UtcNow;
    }
    
    public void Uncomplete()
    {
        IsCompleted = false;
        CompletedAt = null;
    }
} 