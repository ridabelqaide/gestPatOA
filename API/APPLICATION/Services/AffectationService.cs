using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PATOA.APPLICATION.Interfaces;
using PATOA.CORE.Entities;
using PATOA.CORE.Interfaces;
using PATOA.APPLICATION.DTOs;
using Microsoft.EntityFrameworkCore;


namespace PATOA.APPLICATION.Services
{
	public class AffectationService : IAffectationService
	{
        private readonly IAffectationRepository _affectationRepository;

        public AffectationService(IAffectationRepository affectationRepository)
        {
            _affectationRepository = affectationRepository;
        }
        public async Task<IEnumerable<Affectation>> GetAllAsync()
        {
            return await _affectationRepository.GetAllQueryable().ToListAsync();
        }

        public async Task<PagedResult<Affectation>> GetPagedAffectationsAsync(
            Guid? officialId = null, Guid? enginId = null, DateTime? startDate = null, int page = 1, int pageSize = 5
        )
        {
            var query = _affectationRepository.GetAllQueryable();

            if (officialId.HasValue)
                query = query.Where(a => a.OfficialId == officialId.Value);

            if (enginId.HasValue)
                query = query.Where(a => a.EnginId == enginId.Value);

            if (startDate.HasValue)
                query = query.Where(a => a.StartDate.HasValue && a.StartDate.Value.Date == startDate.Value.Date);

           
            var totalItems = await query.CountAsync();
            var data = await query.OrderByDescending(a => a.CreatedOn)
                                  .Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();

            return new PagedResult<Affectation>
            {
                Data = data,
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize
            };
        }
        public async Task<Affectation?> GetByIdAsync(Guid id)
        {
            return await _affectationRepository.GetByIdAsync(id);
        }
        public async Task<Affectation> AddAsync(Affectation affectation)
        {
            return await _affectationRepository.AddAsync(affectation);
        }
        public async Task<Affectation?> UpdateAsync(Affectation affectation)
        {
            var existing = await _affectationRepository.GetByIdAsync(affectation.Id);
            if (existing == null) return null;
            existing.StartDate = affectation.StartDate;
            existing.EndDate = affectation.EndDate;
            existing.CurrentKm = affectation.CurrentKm;
            existing.EndKm = affectation.EndKm;
            existing.Object = affectation.Object;
            existing.Details = affectation.Details;
            existing.EnginId = affectation.EnginId;
            existing.OfficialId = affectation.OfficialId;
            await _affectationRepository.UpdateAsync(existing);
            return existing;
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _affectationRepository.GetByIdAsync(id);
            if (existing == null) return false;
            await _affectationRepository.DeleteAsync(id);
            return true;
        }

    }
}