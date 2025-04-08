using System.ComponentModel.DataAnnotations;

namespace FocusLifePlus.Application.Contracts.Authentication;

public class RefreshTokenRequestDto
{
    [Required]
    public string RefreshToken { get; set; } = null!;
} 