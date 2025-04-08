using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FocusLifePlus.Domain.Common.Enums;
using FocusLifePlus.Domain.Entities;

namespace FocusLifePlus.Domain.Repositories
{
    public interface IFocusTaskRepository
    {
        Task<FocusTask> GetByIdAsync(Guid id);
        Task<List<FocusTask>> GetAllAsync();
        Task<FocusTask> AddAsync(FocusTask task);
        Task UpdateAsync(FocusTask task);
        Task DeleteAsync(FocusTask task);
        
        // Temel filtreleme metotları
        Task<IEnumerable<FocusTask>> GetTasksByUserIdAsync(Guid userId);
        Task<IEnumerable<FocusTask>> GetTasksByStatusAsync(FocusTaskStatus status);
        Task<IEnumerable<FocusTask>> GetTasksByPriorityAsync(FocusTaskPriority priority);
        Task<IEnumerable<FocusTask>> GetTasksByCategoryIdAsync(Guid categoryId);
        
        // Sayfalama ve arama
        Task<(IEnumerable<FocusTask> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
        Task<IEnumerable<FocusTask>> GetTasksBySearchTermAsync(string searchTerm);
        
        // Tarih bazlı sorgular
        Task<IEnumerable<FocusTask>> GetOverdueTasksAsync();
        Task<IEnumerable<FocusTask>> GetTasksDueTodayAsync();
        Task<IEnumerable<FocusTask>> GetTasksDueThisWeekAsync();
        Task<IEnumerable<FocusTask>> GetTasksDueThisMonthAsync();
        Task<IEnumerable<FocusTask>> GetTasksByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task GetByUserIdAsync(Guid userId);
    }
} 