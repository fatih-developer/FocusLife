using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FocusLifePlus.Application.Interfaces.Services;

namespace FocusLifePlus.Infrastructure.Services;

public class PasswordValidationService : IPasswordValidationService
{
    private const int MinLength = 8;
    private const int MaxLength = 128;

    public bool ValidatePassword(string password, out List<string> errors)
    {
        errors = new List<string>();

        if (string.IsNullOrWhiteSpace(password))
        {
            errors.Add("Şifre boş olamaz.");
            return false;
        }

        if (password.Length < MinLength)
            errors.Add($"Şifre en az {MinLength} karakter uzunluğunda olmalıdır.");

        if (password.Length > MaxLength)
            errors.Add($"Şifre en fazla {MaxLength} karakter uzunluğunda olmalıdır.");

        if (!Regex.IsMatch(password, @"[A-Z]"))
            errors.Add("Şifre en az bir büyük harf içermelidir.");

        if (!Regex.IsMatch(password, @"[a-z]"))
            errors.Add("Şifre en az bir küçük harf içermelidir.");

        if (!Regex.IsMatch(password, @"[0-9]"))
            errors.Add("Şifre en az bir rakam içermelidir.");

        if (!Regex.IsMatch(password, @"[!@#$%^&*(),.?""':{}|<>]"))
            errors.Add("Şifre en az bir özel karakter içermelidir.");

        if (HasConsecutiveCharacters(password))
            errors.Add("Şifre ardışık karakterler içermemelidir.");

        if (HasCommonPasswords(password))
            errors.Add("Şifre yaygın olarak kullanılan şifrelerden biri olamaz.");

        return !errors.Any();
    }

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }

    private bool HasConsecutiveCharacters(string password)
    {
        for (int i = 0; i < password.Length - 2; i++)
        {
            if (char.IsLetterOrDigit(password[i]) &&
                char.IsLetterOrDigit(password[i + 1]) &&
                char.IsLetterOrDigit(password[i + 2]))
            {
                if ((password[i + 1] - password[i] == 1 && password[i + 2] - password[i + 1] == 1) ||
                    (password[i] - password[i + 1] == 1 && password[i + 1] - password[i + 2] == 1))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool HasCommonPasswords(string password)
    {
        // Yaygın şifrelerin listesi (örnek olarak)
        var commonPasswords = new HashSet<string>
        {
            "password", "123456", "qwerty", "abc123", "letmein",
            "monkey", "dragon", "baseball", "football", "admin"
        };

        return commonPasswords.Contains(password.ToLower());
    }
} 