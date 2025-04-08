using System.Collections.Generic;

namespace FocusLifePlus.Application.Interfaces.Services;

public interface IPasswordValidationService
{
    bool ValidatePassword(string password, out List<string> errors);
    string HashPassword(string password);
    bool VerifyPassword(string password, string passwordHash);
} 