using System;
using System.Collections.Generic;
using FocusLifePlus.Domain.Common;
using FocusLifePlus.Domain.Enums;

namespace FocusLifePlus.Domain.Entities
{
    public class FocusHabit : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public HabitFrequency Frequency { get; set; }
        public int TargetCount { get; set; }
        public int CurrentStreak { get; set; }
        public int BestStreak { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; } = null!;
        public Guid? FocusCategoryId { get; set; }
        public FocusCategory FocusCategory { get; set; }
        public ICollection<FocusHabitCompletion> Completions { get; set; } = new List<FocusHabitCompletion>();
        public bool IsArchived { get; set; }
    }

    public enum HabitFrequency
    {
        Daily,
        Weekly,
        Monthly
    }
} 