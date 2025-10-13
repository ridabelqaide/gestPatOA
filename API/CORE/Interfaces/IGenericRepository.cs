using PATOA.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PATOA.CORE.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdGuidAsync(Guid id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> Query();
    }
} 