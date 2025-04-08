using System;
using FocusLifePlus.Domain.Common;
using FocusLifePlus.Domain.Common.Enums;

namespace FocusLifePlus.Domain.Entities;

public class FocusTaskAssignment : BaseEntity
{
    public Guid FocusTaskId { get; set; }
    public FocusTask FocusTask { get; set; }
    public Guid AssignedToUserId { get; set; }
    public User AssignedToUser { get; set; }
    public Guid AssignedByUserId { get; set; }
    public User AssignedByUser { get; set; }
    public FocusAssignmentRole Role { get; set; }
    public DateTime AssignedAt { get; set; }
} 