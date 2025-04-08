using FocusLifePlus.Domain.Common;
using System.Collections.Generic;

namespace FocusLifePlus.Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; }
    public string NormalizedName { get; set; }
    public string? Description { get; set; }
    public string? Permissions { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
} 