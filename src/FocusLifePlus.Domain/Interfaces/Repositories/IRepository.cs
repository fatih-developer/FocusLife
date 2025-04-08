using FocusLifePlus.Domain.Common;

namespace FocusLifePlus.Domain.Interfaces.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    Task<T> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
} 