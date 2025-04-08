using System;
using System.Collections.Generic;
using FocusLifePlus.Domain.Common;

namespace FocusLifePlus.Domain.Entities;

public class FocusNote : BaseEntity
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime? UpdatedAt { get; set; }
    public bool IsPinned { get; set; }
    public bool IsArchived { get; set; }
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;
    public virtual ICollection<FocusTag> Tags { get; set; } = new List<FocusTag>();
} 