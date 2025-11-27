using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PATOA.CORE.Entities;
using PATOA.CORE.Interfaces;
using PATOA.APPLICATION.Interfaces;
using PATOA.APPLICATION.DTOs;
using Microsoft.EntityFrameworkCore;

namespace PATOA.APPLICATION.Services
{
    public class OfficialService : IOfficialService
    {
        private readonly IOfficialRepository _officialRepository;

        public OfficialService(IOfficialRepository officialRepository)
        {
            _officialRepository = officialRepository;
        }

        public async Task<IEnumerable<Official>> GetAllOfficialsAsync()
        {
            return await _officialRepository.GetAllAsync();
        }

        public async Task<PagedResult<Official>> GetPagedOfficialsAsync(string? genre, string? fonction, string? service, int page = 1, int pageSize = 5)
        {
            var query = _officialRepository.GetAllQueryable();

            if (!string.IsNullOrEmpty(genre))
                query = query.Where(o => o.Genre.ToLower().Contains(genre.ToLower()));

            if (!string.IsNullOrEmpty(fonction))
                query = query.Where(o => o.Fonction.ToLower().Contains(fonction.ToLower()));

            if (!string.IsNullOrEmpty(service))
                query = query.Where(o => o.Service.ToLower().Contains(service.ToLower()));

            var totalItems = await query.CountAsync();
            var data = await query.OrderByDescending(o => o.CreatedOn)
                                   .Skip((page - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            return new PagedResult<Official>
            {
                Data = data,
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<Official> GetOfficialByIdAsync(Guid id)
        {
            return await _officialRepository.GetByIdAsync(id);
        }

        public async Task<Official> CreateOfficialAsync(Official official)
        {
            return await _officialRepository.AddAsync(official);
        }

        public async Task<string> UpdateOfficialAsync(Guid id, Official official)
        {
            var existing = await _officialRepository.GetByIdAsync(id);
            if (existing == null) throw new KeyNotFoundException($"Aucun official trouvé avec l'ID {id}.");

            await _officialRepository.UpdateAsync(id, official);

            return $"Official '{official.FirstName} {official.LastName}' mis à jour avec succès.";
        }


        public async Task<string> DeleteOfficialAsync(Guid id)
        {
            var existing = await _officialRepository.GetByIdAsync(id);
            if (existing == null) throw new KeyNotFoundException($"Aucun official trouvé avec l'ID {id}.");

            await _officialRepository.DeleteAsync(id);
            return $"Official '{existing.FirstName} {existing.LastName}' supprimé avec succès.";
        }
    }

}

