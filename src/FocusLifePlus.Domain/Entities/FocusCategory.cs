using System;
using System.Collections.Generic;
using FocusLifePlus.Domain.Common;

namespace FocusLifePlus.Domain.Entities;

public class FocusCategory : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    public Guid? ParentFocusCategoryId { get; set; }
    public FocusCategory ParentFocusCategory { get; set; }
    public ICollection<FocusCategory> SubCategories { get; set; }
    public ICollection<FocusTask> Tasks { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
} 