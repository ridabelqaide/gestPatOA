using PATOA.CORE.Interfaces;
using PATOA.CORE.Entities;
using PATOA.INFRA.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace PATOA.INFRA.Repositories
{
    public class PrivatePatRepository : IPrivatePatRepository
    {
        private readonly ApplicationDbContext _context;

        public PrivatePatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PrivatePat>> GetAllAsync(string? registrationNumber = null)
        {
            var query = _context.PrivatePats.AsQueryable();

            if (!string.IsNullOrEmpty(registrationNumber))
            {
                query = query.Where(p => p.RegistrationNumber.Contains(registrationNumber));
            }

            return await query.ToListAsync();

        }

        public async Task<PrivatePat?> GetByIdAsync(Guid id)
        {
            return await _context.PrivatePats.FindAsync(id);
        }

        public async Task<PrivatePat> AddAsync(PrivatePat entity)
        {
            _context.PrivatePats.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(PrivatePat existing, PrivatePat updated)
        {
            updated.Id = existing.Id;

            _context.Entry(existing).CurrentValues.SetValues(updated);

            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.PrivatePats.FindAsync(id);
            if (entity != null)
            {
                _context.PrivatePats.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}