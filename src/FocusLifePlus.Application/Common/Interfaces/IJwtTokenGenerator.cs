using FocusLifePlus.Domain.Entities;

namespace FocusLifePlus.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user, IList<string> roles);
    string GenerateRefreshToken();
    DateTime GetRefreshTokenExpiryTime();
} 