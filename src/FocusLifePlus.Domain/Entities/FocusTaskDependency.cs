using System;
using FocusLifePlus.Domain.Common;
using FocusLifePlus.Domain.Common.Enums;

namespace FocusLifePlus.Domain.Entities;

public class FocusTaskDependency : BaseEntity
{
    public Guid DependentTaskId { get; set; }
    public FocusTask DependentTask { get; set; }
    public Guid DependencyTaskId { get; set; }
    public FocusTask DependencyTask { get; set; }
    public FocusDependencyType Type { get; set; }
} 