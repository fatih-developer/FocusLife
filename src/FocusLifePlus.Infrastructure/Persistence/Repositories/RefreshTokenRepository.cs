using System;
using System.Threading.Tasks;
using FocusLifePlus.Application.Interfaces.Repositories;
using FocusLifePlus.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FocusLifePlus.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _context;

    public RefreshTokenRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == token && !rt.IsDeleted);
    }

    public async Task<RefreshToken?> GetByUserIdAsync(Guid userId)
    {
        return await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.UserId == userId && 
                                     !rt.IsDeleted && 
                                     rt.Expires > DateTime.UtcNow);
    }

    public async Task AddAsync(RefreshToken refreshToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task RevokeAsync(string token, string? replacedByToken = null)
    {
        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token && !rt.IsDeleted);

        if (refreshToken != null)
        {
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.ReplacedByToken = replacedByToken;
            refreshToken.IsDeleted = true;
            refreshToken.DeletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }

    public async Task RevokeAllAsync(Guid userId, string? exceptToken = null)
    {
        var refreshTokens = await _context.RefreshTokens
            .Where(rt => rt.UserId == userId && 
                        !rt.IsDeleted &&
                        (exceptToken == null || rt.Token != exceptToken))
            .ToListAsync();

        foreach (var refreshToken in refreshTokens)
        {
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.IsDeleted = true;
            refreshToken.DeletedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
    }

    public async Task RemoveExpiredTokensAsync(Guid userId)
    {
        var expiredTokens = await _context.RefreshTokens
            .Where(rt => rt.UserId == userId && 
                        !rt.IsDeleted && 
                        rt.Expires <= DateTime.UtcNow)
            .ToListAsync();

        foreach (var token in expiredTokens)
        {
            token.IsDeleted = true;
            token.DeletedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
    }
} 