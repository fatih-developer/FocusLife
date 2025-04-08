using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FocusLifePlus.Domain.Entities;

namespace FocusLifePlus.Application.Interfaces.Repositories;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(Guid id);
    Task<Role?> GetByNameAsync(string normalizedName);
    Task<IList<Role>> GetUserRolesAsync(Guid userId);
    Task<bool> IsInRoleAsync(Guid userId, string roleName);
    Task AddToRoleAsync(Guid userId, Guid roleId);
    Task RemoveFromRoleAsync(Guid userId, Guid roleId);
    Task<IList<Role>> GetAllAsync();
} 