using System;
using FocusLifePlus.Domain.Common;

namespace FocusLifePlus.Domain.Entities;

public class FocusReminder : BaseEntity
{
    public DateTime ReminderTime { get; set; }
    public string Message { get; set; }
    public bool IsCompleted { get; set; }
    public Guid TaskId { get; set; }
    public FocusTask Task { get; set; }
} 