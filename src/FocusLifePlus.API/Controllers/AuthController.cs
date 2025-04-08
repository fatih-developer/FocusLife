using System;
using System.Linq;
using System.Threading.Tasks;
using FocusLifePlus.Application.Contracts.Authentication;
using FocusLifePlus.Application.Interfaces.Repositories;
using FocusLifePlus.Application.Interfaces.Services;
using FocusLifePlus.Infrastructure.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FocusLifePlus.API.Controllers;

/// <summary>
/// Kimlik doğrulama ve yetkilendirme işlemlerini yöneten controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IRoleRepository _roleRepository;
    private readonly JwtOptions _jwtOptions;

    public AuthController(
        IAuthService authService,
        IRoleRepository roleRepository,
        IOptions<JwtOptions> jwtOptions)
    {
        _authService = authService;
        _roleRepository = roleRepository;
        _jwtOptions = jwtOptions.Value;
    }

    /// <summary>
    /// Yeni bir kullanıcı kaydı oluşturur
    /// </summary>
    /// <param name="request">Kullanıcı kayıt bilgileri</param>
    /// <returns>Kayıt başarılı ise kullanıcı bilgileri ve token'lar</returns>
    /// <response code="200">Kayıt başarılı</response>
    /// <response code="400">Geçersiz istek veya validasyon hatası</response>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto request)
    {
        try
        {
            var result = await _authService.RegisterAsync(request);
            SetRefreshTokenCookie(result.RefreshToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Mevcut kullanıcı girişi yapar
    /// </summary>
    /// <param name="request">Kullanıcı giriş bilgileri</param>
    /// <returns>Giriş başarılı ise kullanıcı bilgileri ve token'lar</returns>
    /// <response code="200">Giriş başarılı</response>
    /// <response code="400">Geçersiz kullanıcı adı veya şifre</response>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] UserLoginDto request)
    {
        try
        {
            var result = await _authService.LoginAsync(request);
            SetRefreshTokenCookie(result.RefreshToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Access token'ı yeniler
    /// </summary>
    /// <returns>Yeni access token ve refresh token</returns>
    /// <response code="200">Token yenileme başarılı</response>
    /// <response code="400">Geçersiz veya süresi dolmuş refresh token</response>
    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Refresh()
    {
        try
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest(new { message = "Refresh token bulunamadı." });

            var request = new RefreshTokenRequestDto { RefreshToken = refreshToken };
            var result = await _authService.RefreshTokenAsync(request);
            SetRefreshTokenCookie(result.RefreshToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Refresh token'ı iptal eder
    /// </summary>
    /// <returns>İptal işlemi sonucu</returns>
    /// <response code="200">Token başarıyla iptal edildi</response>
    /// <response code="400">Geçersiz token</response>
    /// <response code="401">Yetkisiz erişim</response>
    [HttpPost("revoke")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Revoke()
    {
        try
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest(new { message = "Refresh token bulunamadı." });

            await _authService.RevokeTokenAsync(refreshToken);
            Response.Cookies.Delete("refreshToken");
            return Ok(new { message = "Token başarıyla iptal edildi." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private void SetRefreshTokenCookie(string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenTTL)
        };
        Response.Cookies.Append("refreshToken", token, cookieOptions);
    }

    /// <summary>
    /// Mevcut kullanıcının bilgilerini getirir
    /// </summary>
    /// <returns>Kullanıcı bilgileri</returns>
    /// <response code="200">Kullanıcı bilgileri başarıyla getirildi</response>
    /// <response code="401">Yetkisiz erişim</response>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetCurrentUser()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var username = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
        var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        var roles = User.FindAll(System.Security.Claims.ClaimTypes.Role).Select(c => c.Value).ToList();

        return Ok(new
        {
            Id = userId,
            Username = username,
            Email = email,
            Roles = roles
        });
    }

    /// <summary>
    /// Kullanıcıya rol atar (Sadece Admin rolüne sahip kullanıcılar)
    /// </summary>
    /// <param name="request">Rol atama bilgileri</param>
    /// <returns>Atama işlemi sonucu</returns>
    /// <response code="200">Rol başarıyla atandı</response>
    /// <response code="400">Geçersiz istek</response>
    /// <response code="401">Yetkisiz erişim</response>
    /// <response code="403">Yetersiz yetki (Admin rolü gerekli)</response>
    [HttpPost("assign-role")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequestDto request)
    {
        try
        {
            await _roleRepository.AddToRoleAsync(request.UserId, request.RoleId);
            return Ok(new { message = "Rol başarıyla atandı." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
} 