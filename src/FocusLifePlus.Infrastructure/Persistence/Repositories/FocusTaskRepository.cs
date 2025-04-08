using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FocusLifePlus.Domain.Entities;
using FocusLifePlus.Domain.Common.Enums;
using FocusLifePlus.Domain.Repositories;

namespace FocusLifePlus.Infrastructure.Persistence.Repositories
{
    public class FocusTaskRepository : IFocusTaskRepository
    {
        private readonly ApplicationDbContext _context;

        public FocusTaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FocusTask> GetByIdAsync(Guid id)
        {
            return await _context.FocusTasks
                .Include(t => t.FocusCategory)
                .Include(t => t.SubTasks)
                .Include(t => t.Tags)
                .Include(t => t.Reminders)
                .Include(t => t.History)
                .Include(t => t.Comments)
                .Include(t => t.Assignments)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<FocusTask>> GetAllAsync()
        {
            return await _context.FocusTasks
                .Include(t => t.FocusCategory)
                .Include(t => t.SubTasks)
                .ToListAsync();
        }

        public async Task<IEnumerable<FocusTask>> GetTasksByUserIdAsync(Guid userId)
        {
            return await _context.FocusTasks
                .Include(t => t.FocusCategory)
                .Include(t => t.SubTasks)
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<FocusTask>> GetTasksByCategoryIdAsync(Guid categoryId)
        {
            return await _context.FocusTasks
                .Include(t => t.SubTasks)
                .Where(t => t.FocusCategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<FocusTask>> GetTasksByStatusAsync(FocusTaskStatus status)
        {
            return await _context.FocusTasks
                .Include(t => t.FocusCategory)
                .Include(t => t.SubTasks)
                .Where(t => t.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<FocusTask>> GetOverdueTasksAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.FocusTasks
                .Include(t => t.FocusCategory)
                .Include(t => t.SubTasks)
                .Where(t => !t.IsCompleted && t.DueDate < now)
                .ToListAsync();
        }

        public async Task<IEnumerable<FocusTask>> GetTasksDueTodayAsync()
        {
            var today = DateTime.UtcNow.Date;
            return await _context.FocusTasks
                .Include(t => t.FocusCategory)
                .Include(t => t.SubTasks)
                .Where(t => !t.IsCompleted && t.DueDate.Date == today)
                .ToListAsync();
        }

        public async Task<IEnumerable<FocusTask>> GetTasksDueThisWeekAsync()
        {
            var now = DateTime.UtcNow;
            var weekEnd = now.AddDays(7);
            return await _context.FocusTasks
                .Include(t => t.FocusCategory)
                .Include(t => t.SubTasks)
                .Where(t => !t.IsCompleted && t.DueDate >= now && t.DueDate <= weekEnd)
                .ToListAsync();
        }

        public async Task<FocusTask> AddAsync(FocusTask task)
        {
            await _context.FocusTasks.AddAsync(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task UpdateAsync(FocusTask task)
        {
            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(FocusTask task)
        {
            _context.FocusTasks.Remove(task);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.FocusTasks.AnyAsync(t => t.Id == id);
        }

        public async Task<int> GetTaskCountByUserIdAsync(Guid userId)
        {
            return await _context.FocusTasks.CountAsync(t => t.UserId == userId);
        }

        public async Task<IEnumerable<FocusTask>> GetTasksWithDependenciesAsync(Guid taskId)
        {
            return await _context.FocusTasks
                .Include(t => t.Dependencies)
                    .ThenInclude(d => d.DependencyTask)
                .Include(t => t.Dependents)
                    .ThenInclude(d => d.DependentTask)
                .Where(t => t.Id == taskId)
                .ToListAsync();
        }

        public async Task<IEnumerable<FocusTask>> GetTasksByPriorityAsync(FocusTaskPriority priority)
        {
            return await _context.FocusTasks
                .Where(t => t.Priority == priority)
                .ToListAsync();
        }

        public async Task<(IEnumerable<FocusTask> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.FocusTasks.AsQueryable();
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<IEnumerable<FocusTask>> GetTasksBySearchTermAsync(string searchTerm)
        {
            return await _context.FocusTasks
                .Where(t => t.Title.Contains(searchTerm) || t.Description.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<IEnumerable<FocusTask>> GetTasksDueThisMonthAsync()
        {
            var today = DateTime.UtcNow.Date;
            var endOfMonth = today.AddMonths(1);
            return await _context.FocusTasks
                .Where(t => !t.IsCompleted && t.DueDate.Date >= today && t.DueDate.Date <= endOfMonth)
                .ToListAsync();
        }

        public async Task<IEnumerable<FocusTask>> GetTasksByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.FocusTasks
                .Where(t => t.DueDate >= startDate && t.DueDate <= endDate)
                .ToListAsync();
        }

        public async Task GetByUserIdAsync(Guid userId)
        {
            await GetTasksByUserIdAsync(userId);
        }
    }
} 