using Microsoft.EntityFrameworkCore;
using PATOA.CORE.Entities;
using PATOA.INFRA.Data;

namespace PATOA.INFRA.Repositories
{
    public class EnginRepository
    {
        private readonly ApplicationDbContext _context;

        public EnginRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Engin>> GetAllAsync()
        {
            return await _context.Engins.ToListAsync();
        }

        public async Task<Engin> GetByIdAsync(Guid id)
        {
            return await _context.Engins.FindAsync(id);
        }

        public async Task<Engin> AddAsync(Engin engin)
        {
            engin.Id = Guid.NewGuid();
            _context.Engins.Add(engin);
            await _context.SaveChangesAsync();
            return engin;
        }

        public async Task UpdateAsync(Engin engin)
        {
            _context.Entry(engin).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var engin = await _context.Engins.FindAsync(id);
            if (engin != null)
            {
                _context.Engins.Remove(engin);
                await _context.SaveChangesAsync();
            }
        }
    }
}
