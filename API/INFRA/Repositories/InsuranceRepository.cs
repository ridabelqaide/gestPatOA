using Microsoft.EntityFrameworkCore;
using PATOA.CORE.Entities;
using PATOA.CORE.Interfaces;
using PATOA.INFRA.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PATOA.INFRA.Repositories
{
    public class InsuranceRepository : IInsuranceRepository
    {
        private readonly ApplicationDbContext _context;

        public InsuranceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Insurance>> GetAllAsync()
        {
            return await _context.Insurances.Include(i => i.Engin).ToListAsync();
        }

        public async Task<Insurance?> GetByIdAsync(Guid id)
        {
            return await _context.Insurances.Include(i => i.Engin)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task AddAsync(Insurance insurance)
        {
            await _context.Insurances.AddAsync(insurance);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Insurance insurance)
        {
            _context.Insurances.Update(insurance);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var insurance = await _context.Insurances.FindAsync(id);
            if (insurance != null)
            {
                _context.Insurances.Remove(insurance);
                await _context.SaveChangesAsync();
            }
        }


    }
}

