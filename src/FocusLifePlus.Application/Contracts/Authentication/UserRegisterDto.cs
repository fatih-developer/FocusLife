using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FocusLifePlus.Application.Contracts.Authentication;

public class UserRegisterDto
{
    [Required]
    [StringLength(50)]
    public string Username { get; set; } = null!;

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = null!;

    public List<string> Roles { get; set; } = new() { "User" };
} 