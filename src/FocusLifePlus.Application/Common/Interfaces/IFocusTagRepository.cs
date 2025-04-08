using FocusLifePlus.Domain.Entities;

namespace FocusLifePlus.Application.Common.Interfaces;

public interface IFocusTagRepository
{
    Task<FocusTag> GetByIdAsync(Guid id);
    Task<List<FocusTag>> GetUserTagsAsync(Guid userId);
    Task<List<FocusTag>> GetTagsByTaskAsync(Guid taskId);
    Task<FocusTag> AddAsync(FocusTag tag);
    Task UpdateAsync(FocusTag tag);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> HasTasksAsync(Guid id);
} 