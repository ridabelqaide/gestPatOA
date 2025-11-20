using Microsoft.EntityFrameworkCore;
using PATOA.CORE.Entities;
using PATOA.CORE.Interfaces;
using PATOA.INFRA.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PATOA.INFRA.Repositories
{
    public class EnginTypeRepository : IEnginTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public EnginTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EnginType>> GetAllAsync()
        {
            return await _context.EnginTypes.ToListAsync();
        }

        public async Task<EnginType?> GetByCodeAsync(string code)
        {
            return await _context.EnginTypes.FindAsync(code);
        }

        public async Task<EnginType> AddAsync(EnginType type)
        {
            _context.EnginTypes.Add(type);
            await _context.SaveChangesAsync();
            return type;
        }

        public async Task UpdateAsync(EnginType type)
        {
            _context.EnginTypes.Update(type);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string code)
        {
            var type = await GetByCodeAsync(code);
            if (type != null)
            {
                _context.EnginTypes.Remove(type);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(string code)
        {
            return await _context.EnginTypes.AnyAsync(t => t.Code == code);
        }

        public async Task<(IEnumerable<EnginType> Data, int TotalCount)> GetAllAsync(string? name, int pageNumber, int pageSize)
        {
            var query = _context.EnginTypes.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(e => e.Name.Contains(name));
            query = query.OrderByDescending(e => e.Name);
            var totalCount = await query.CountAsync();
            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, totalCount);
        }
    }
}
