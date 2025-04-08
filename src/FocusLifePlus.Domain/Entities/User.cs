using System;
using System.Collections.Generic;
using FocusLifePlus.Domain.Common;

namespace FocusLifePlus.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = string.Empty;
        public string NormalizedUsername { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NormalizedEmail { get; set; } = string.Empty;
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual ICollection<FocusTask> Tasks { get; set; } = new List<FocusTask>();
        public virtual ICollection<FocusNote> Notes { get; set; } = new List<FocusNote>();
        public virtual ICollection<FocusHabit> Habits { get; set; } = new List<FocusHabit>();
        public virtual ICollection<FocusGoal> Goals { get; set; } = new List<FocusGoal>();
        public virtual ICollection<FocusBudget> Budgets { get; set; } = new List<FocusBudget>();
        public virtual ICollection<FocusCategory> Categories { get; set; } = new List<FocusCategory>();
    }
} 