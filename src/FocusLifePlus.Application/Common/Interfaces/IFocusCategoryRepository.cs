using FocusLifePlus.Domain.Entities;

namespace FocusLifePlus.Application.Common.Interfaces;

public interface IFocusCategoryRepository : IGenericRepository<FocusCategory>
{
    Task<FocusCategory> GetByIdAsync(Guid id);
    Task<List<FocusCategory>> GetUserCategoriesAsync(Guid userId);
    Task<List<FocusCategory>> GetSubCategoriesAsync(Guid parentCategoryId);
    Task<FocusCategory> AddAsync(FocusCategory category);
    Task UpdateAsync(FocusCategory category);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> HasSubCategoriesAsync(Guid id);
    Task<bool> HasTasksAsync(Guid id);
} 