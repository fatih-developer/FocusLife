using System;
using System.Collections.Generic;
using FocusLifePlus.Domain.Common;

namespace FocusLifePlus.Domain.Entities;

public class FocusTag : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Color { get; set; } = null!;
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;
    public virtual ICollection<FocusTask> Tasks { get; set; } = new List<FocusTask>();
    public virtual ICollection<FocusNote> Notes { get; set; } = new List<FocusNote>();
} 