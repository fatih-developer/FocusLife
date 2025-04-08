using FocusLifePlus.Domain.Common.Enums;
using FocusLifePlus.Domain.Entities;

namespace FocusLifePlus.Application.Common.Interfaces;

public interface ITaskRepository
{
    Task<IEnumerable<FocusTask>> GetAllByUserIdAsync(Guid userId);
    Task<FocusTask> GetByIdAsync(Guid taskId);
    Task<IEnumerable<FocusTask>> GetByStatusAsync(Guid userId, FocusTaskStatus status);
    Task<FocusTask> AddAsync(FocusTask task);
    Task UpdateAsync(FocusTask task);
    Task DeleteAsync(FocusTask task);
} 