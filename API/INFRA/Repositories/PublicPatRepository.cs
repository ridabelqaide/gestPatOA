using PATOA.CORE.Entities;
using PATOA.INFRA.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using PATOA.CORE.Interfaces;

namespace PATOA.INFRA.Repositories
{
    public class PublicPatRepository : IPublicPatRepository
    {
        private readonly ApplicationDbContext _context;

        public PublicPatRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<PublicPat>> GetAllAsync(string? registrationNumber = null)
        {
            var query = _context.PublicPats.AsQueryable();

            if (!string.IsNullOrEmpty(registrationNumber))
            {
                query = query.Where(p => p.RegistrationNumber.Contains(registrationNumber));
            }

            return await query.ToListAsync();
        }

        public async Task<PublicPat?> GetByIdAsync(Guid id)
        {
            return await _context.PublicPats.FindAsync(id);
        }

        public async Task CreateAsync(PublicPat entity)
        {
            await _context.PublicPats.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PublicPat existing, PublicPat updated)
        {
            updated.Id = existing.Id;

            _context.Entry(existing).CurrentValues.SetValues(updated);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(PublicPat entity)
        {
            _context.PublicPats.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
