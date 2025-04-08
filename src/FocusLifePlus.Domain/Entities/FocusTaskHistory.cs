using System;
using FocusLifePlus.Domain.Common;

namespace FocusLifePlus.Domain.Entities;

public class FocusTaskHistory : BaseEntity
{
    public string FieldName { get; set; }
    public string OldValue { get; set; }
    public string NewValue { get; set; }
    public DateTime ChangedAt { get; set; }
    public Guid FocusTaskId { get; set; }
    public FocusTask FocusTask { get; set; }
    public Guid? ChangedByUserId { get; set; }
    public User ChangedByUser { get; set; }
} 