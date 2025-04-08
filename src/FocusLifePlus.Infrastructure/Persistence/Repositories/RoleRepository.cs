using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FocusLifePlus.Application.Interfaces.Repositories;
using FocusLifePlus.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FocusLifePlus.Infrastructure.Persistence.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly ApplicationDbContext _context;

    public RoleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Role?> GetByIdAsync(Guid id)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
    }

    public async Task<Role?> GetByNameAsync(string normalizedName)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.NormalizedName == normalizedName && !r.IsDeleted);
    }

    public async Task<IList<Role>> GetUserRolesAsync(Guid userId)
    {
        var userRoles = await _context.UserRoles
            .Include(ur => ur.Role)
            .Where(ur => ur.UserId == userId && !ur.IsDeleted && !ur.Role.IsDeleted)
            .Select(ur => ur.Role)
            .ToListAsync();

        return userRoles;
    }

    public async Task<bool> IsInRoleAsync(Guid userId, string roleName)
    {
        var normalizedRoleName = roleName.ToUpper();
        return await _context.UserRoles
            .Include(ur => ur.Role)
            .AnyAsync(ur => ur.UserId == userId && 
                          !ur.IsDeleted && 
                          !ur.Role.IsDeleted && 
                          ur.Role.NormalizedName == normalizedRoleName);
    }

    public async Task AddToRoleAsync(Guid userId, Guid roleId)
    {
        var userRole = new UserRole
        {
            UserId = userId,
            RoleId = roleId
        };

        await _context.UserRoles.AddAsync(userRole);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveFromRoleAsync(Guid userId, Guid roleId)
    {
        var userRole = await _context.UserRoles
            .FirstOrDefaultAsync(ur => ur.UserId == userId && 
                                     ur.RoleId == roleId && 
                                     !ur.IsDeleted);

        if (userRole != null)
        {
            userRole.IsDeleted = true;
            userRole.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IList<Role>> GetAllAsync()
    {
        return await _context.Roles
            .Where(r => !r.IsDeleted)
            .ToListAsync();
    }
} 