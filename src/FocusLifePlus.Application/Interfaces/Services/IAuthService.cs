using System.Collections.Generic;
using System.Threading.Tasks;
using FocusLifePlus.Application.Contracts.Authentication;
using FocusLifePlus.Domain.Entities;

namespace FocusLifePlus.Application.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(UserRegisterDto request);
    Task<AuthResponseDto> LoginAsync(UserLoginDto request);
    Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request);
    Task RevokeTokenAsync(string token);
    Task<string> CreateAccessTokenAsync(User user, IList<Role> roles);
    Task<string> GenerateRefreshTokenAsync();
} 