using System;
using System.Threading.Tasks;
using FocusLifePlus.Domain.Entities;

namespace FocusLifePlus.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByRefreshTokenAsync(string refreshToken);
    Task<bool> IsUsernameUniqueAsync(string username);
    Task<bool> IsEmailUniqueAsync(string email);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
} 