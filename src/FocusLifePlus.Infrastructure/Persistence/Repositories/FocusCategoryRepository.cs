using FocusLifePlus.Application.Common.Interfaces;
using FocusLifePlus.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FocusLifePlus.Infrastructure.Persistence.Repositories;

public class FocusCategoryRepository : GenericRepository<FocusCategory>, IFocusCategoryRepository
{
    public FocusCategoryRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<FocusCategory>> GetUserCategoriesAsync(Guid userId)
    {
        return await _dbSet
            .Include(c => c.SubCategories)
            .Include(c => c.Tasks)
            .Where(c => c.UserId == userId && !c.IsDeleted)
            .ToListAsync();
    }

    public async Task<List<FocusCategory>> GetSubCategoriesAsync(Guid parentCategoryId)
    {
        return await _dbSet
            .Include(c => c.Tasks)
            .Where(c => c.ParentFocusCategoryId == parentCategoryId && !c.IsDeleted)
            .ToListAsync();
    }

    public override async Task<FocusCategory?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(c => c.SubCategories)
            .Include(c => c.Tasks)
            .Include(c => c.ParentFocusCategory)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
    }

    public async Task<bool> HasSubCategoriesAsync(Guid id)
    {
        return await _dbSet.AnyAsync(c => c.ParentFocusCategoryId == id && !c.IsDeleted);
    }

    public async Task<bool> HasTasksAsync(Guid id)
    {
        var category = await _dbSet
            .Include(c => c.Tasks)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

        return category?.Tasks?.Any(t => !t.IsDeleted) ?? false;
    }
} 