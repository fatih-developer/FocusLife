using FocusLifePlus.Application.Common.Interfaces;
using FocusLifePlus.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FocusLifePlus.Infrastructure.Persistence.Repositories;

public class FocusTagRepository : GenericRepository<FocusTag>, IFocusTagRepository
{
    public FocusTagRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<FocusTag>> GetUserTagsAsync(Guid userId)
    {
        return await _dbSet
            .Include(t => t.Tasks)
            .Where(t => t.UserId == userId && !t.IsDeleted)
            .ToListAsync();
    }

    public async Task<List<FocusTag>> GetTagsByTaskAsync(Guid taskId)
    {
        return await _dbSet
            .Include(t => t.Tasks)
            .Where(t => t.Tasks.Any(task => task.Id == taskId) && !t.IsDeleted)
            .ToListAsync();
    }

    public override async Task<FocusTag> GetByIdAsync(Guid id)
    {
        var entity = await _dbSet
            .Include(t => t.Tasks)
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

        return entity ?? throw new KeyNotFoundException($"FocusTag with ID {id} not found.");
    }

    public async Task<bool> HasTasksAsync(Guid id)
    {
        var tag = await _dbSet
            .Include(t => t.Tasks)
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

        return tag?.Tasks?.Any(t => !t.IsDeleted) ?? false;
    }
} 