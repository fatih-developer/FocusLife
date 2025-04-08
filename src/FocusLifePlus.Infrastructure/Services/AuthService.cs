using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FocusLifePlus.Application.Contracts.Authentication;
using FocusLifePlus.Application.Interfaces.Repositories;
using FocusLifePlus.Application.Interfaces.Services;
using FocusLifePlus.Domain.Entities;
using FocusLifePlus.Infrastructure.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FocusLifePlus.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly JwtOptions _jwtOptions;

    public AuthService(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IOptions<JwtOptions> jwtOptions)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<AuthResponseDto> RegisterAsync(UserRegisterDto request)
    {
        // Kullanıcı adı ve email kontrolü
        if (!await _userRepository.IsUsernameUniqueAsync(request.Username))
            throw new Exception("Bu kullanıcı adı zaten kullanılıyor.");

        if (!await _userRepository.IsEmailUniqueAsync(request.Email))
            throw new Exception("Bu e-posta adresi zaten kullanılıyor.");

        // Şifre hashleme
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // Yeni kullanıcı oluşturma
        var user = new User
        {
            Username = request.Username,
            NormalizedUsername = request.Username.ToUpper(),
            Email = request.Email,
            NormalizedEmail = request.Email.ToUpper(),
            PasswordHash = passwordHash,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        await _userRepository.AddAsync(user);

        // Varsayılan "User" rolünü atama
        var userRole = await _roleRepository.GetByNameAsync("USER");
        if (userRole != null)
        {
            await _roleRepository.AddToRoleAsync(user.Id, userRole.Id);
        }

        // Kullanıcı rollerini alma
        var roles = await _roleRepository.GetUserRolesAsync(user.Id);

        // Access token ve refresh token oluşturma
        string accessToken = await CreateAccessTokenAsync(user, roles);
        string refreshToken = await GenerateRefreshTokenAsync();

        // Refresh token'ı kaydetme
        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            Created = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenTTL)
        };

        await _refreshTokenRepository.AddAsync(refreshTokenEntity);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Username = user.Username,
            Email = user.Email,
            Roles = roles.Select(r => r.Name).ToList()
        };
    }

    public async Task<AuthResponseDto> LoginAsync(UserLoginDto request)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);
        if (user == null)
            throw new Exception("Kullanıcı adı veya şifre hatalı.");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new Exception("Kullanıcı adı veya şifre hatalı.");

        var roles = await _roleRepository.GetUserRolesAsync(user.Id);
        string accessToken = await CreateAccessTokenAsync(user, roles);
        string refreshToken = await GenerateRefreshTokenAsync();

        // Eski refresh token'ları iptal et
        await _refreshTokenRepository.RevokeAllAsync(user.Id);

        // Yeni refresh token'ı kaydet
        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            Created = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenTTL)
        };

        await _refreshTokenRepository.AddAsync(refreshTokenEntity);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Username = user.Username,
            Email = user.Email,
            Roles = roles.Select(r => r.Name).ToList()
        };
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
    {
        var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
        if (refreshToken == null)
            throw new Exception("Geçersiz refresh token.");

        if (refreshToken.Expires <= DateTime.UtcNow)
            throw new Exception("Refresh token süresi dolmuş.");

        if (refreshToken.Revoked.HasValue)
            throw new Exception("Refresh token iptal edilmiş.");

        var user = refreshToken.User;
        var roles = await _roleRepository.GetUserRolesAsync(user.Id);
        string accessToken = await CreateAccessTokenAsync(user, roles);
        string newRefreshToken = await GenerateRefreshTokenAsync();

        // Eski token'ı iptal et ve yenisini kaydet
        await _refreshTokenRepository.RevokeAsync(refreshToken.Token, newRefreshToken);

        var newRefreshTokenEntity = new RefreshToken
        {
            Token = newRefreshToken,
            UserId = user.Id,
            Created = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenTTL)
        };

        await _refreshTokenRepository.AddAsync(newRefreshTokenEntity);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            Username = user.Username,
            Email = user.Email,
            Roles = roles.Select(r => r.Name).ToList()
        };
    }

    public async Task RevokeTokenAsync(string token)
    {
        var refreshToken = await _refreshTokenRepository.GetByTokenAsync(token);
        if (refreshToken == null)
            throw new Exception("Geçersiz refresh token.");

        if (refreshToken.Revoked.HasValue)
            throw new Exception("Token zaten iptal edilmiş.");

        await _refreshTokenRepository.RevokeAsync(token);
    }

    public async Task<string> CreateAccessTokenAsync(User user, IList<Role> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Rol claim'lerini ekle
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Name));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(_jwtOptions.TokenExpirationInMinutes);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> GenerateRefreshTokenAsync()
    {
        var randomBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
} 