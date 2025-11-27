using PATOA.CORE.Entities;
using PATOA.CORE.Interfaces;
using PATOA.INFRA.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace PATOA.INFRA.Repositories
{
    public class AffectationRepository : IAffectationRepository
    {
        private readonly ApplicationDbContext _context;

        public AffectationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Affectation> GetAllQueryable()
        {
            return _context.Affectations
                .Include(a => a.Engin)
                .Include(a => a.Official)
                .AsQueryable();
        }


        public async Task<Affectation?> GetByIdAsync(Guid id)
        {
            return await _context.Affectations.FindAsync(id);
        }

        public async Task<Affectation> AddAsync(Affectation affectation)
        {
            affectation.Id = Guid.NewGuid();
            _context.Affectations.Add(affectation);
            await _context.SaveChangesAsync();
            return affectation;
        }

        public async Task UpdateAsync(Affectation affectation)
        {
            var existing = await _context.Affectations.FindAsync(affectation.Id);
            if (existing == null) throw new KeyNotFoundException($"Aucune affectation trouvée avec l'ID {affectation.Id}.");

            existing.StartDate = affectation.StartDate;
            existing.EndDate = affectation.EndDate;
            existing.CurrentKm = affectation.CurrentKm;
            existing.EndKm = affectation.EndKm;
            existing.Object = affectation.Object;  
            existing.Details = affectation.Details;
            existing.EnginId = affectation.EnginId;
            existing.OfficialId = affectation.OfficialId;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var existing = await _context.Affectations.FindAsync(id);
            if (existing == null) throw new KeyNotFoundException($"Aucune affectation trouvée avec l'ID {id}.");

            _context.Affectations.Remove(existing);
            await _context.SaveChangesAsync();
        }
    }
}
