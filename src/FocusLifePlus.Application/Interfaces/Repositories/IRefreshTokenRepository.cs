using System;
using System.Threading.Tasks;
using FocusLifePlus.Domain.Entities;

namespace FocusLifePlus.Application.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task<RefreshToken?> GetByUserIdAsync(Guid userId);
    Task AddAsync(RefreshToken refreshToken);
    Task RevokeAsync(string token, string? replacedByToken = null);
    Task RevokeAllAsync(Guid userId, string? exceptToken = null);
    Task RemoveExpiredTokensAsync(Guid userId);
} 