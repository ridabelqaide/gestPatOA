using PATOA.CORE.Entities;
using PATOA.CORE.Interfaces;
using PATOA.INFRA.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PATOA.INFRA.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;
        protected readonly IUnitOfWork _unitOfWork;

        public GenericRepository(ApplicationDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _dbSet = context.Set<T>();
            _unitOfWork = unitOfWork;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<T> GetByIdGuidAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _unitOfWork.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public virtual IQueryable<T> Query()
        {
            return _dbSet.AsQueryable();
        }
    }
} 