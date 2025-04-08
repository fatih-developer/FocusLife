using System.ComponentModel.DataAnnotations;

namespace FocusLifePlus.Application.Contracts.Authentication;

public class UserLoginDto
{
    [Required]
    [StringLength(50)]
    public string Username { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Password { get; set; } = null!;
} 