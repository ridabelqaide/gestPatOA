using System;
using PATOA.INFRA.Data;
using PATOA.CORE.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using PATOA.CORE.Interfaces;
using System.Linq;



namespace PATOA.INFRA.Repositories
{
    public class OfficialRepository : IOfficialRepository
    {
        private readonly ApplicationDbContext _context;

        public OfficialRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Official>> GetAllAsync()
        {
            return await _context.Officials.ToListAsync();
        }
        public IQueryable<Official> GetAllQueryable()
        {
            return _context.Officials.AsQueryable();
        }
        public async Task<Official> GetByIdAsync(Guid id)
        {
            return await _context.Officials.FindAsync(id);
        }
        public async Task<Official> AddAsync(Official official)
        {
            official.Id = Guid.NewGuid();
            _context.Officials.Add(official);
            await _context.SaveChangesAsync();
            return official;
        }
        public async Task UpdateAsync(Guid id, Official official)
        {
            var existing = await _context.Officials.FindAsync(id);
            if (existing == null) throw new KeyNotFoundException($"Aucun official trouvé avec l'ID {id}.");

            existing.FirstName = official.FirstName;
            existing.LastName = official.LastName;
            existing.CIN = official.CIN;
            existing.Fonction = official.Fonction;
            existing.Genre = official.Genre;
            existing.DateOfBirth = official.DateOfBirth;
            existing.DateEmbauche = official.DateEmbauche;
            existing.Service = official.Service;
            existing.Details = official.Details;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var official = await _context.Officials.FindAsync(id);
            if (official != null)
            {
                _context.Officials.Remove(official);
                await _context.SaveChangesAsync();
            }
        }

    }
}

