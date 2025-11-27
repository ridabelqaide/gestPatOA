using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PATOA.CORE.Entities;
using PATOA.APPLICATION.DTOs;

namespace PATOA.APPLICATION.Interfaces
{
    public interface IAffectationService
    {
        Task<IEnumerable<Affectation>> GetAllAsync();

        Task<PagedResult<Affectation>> GetPagedAffectationsAsync(
            Guid? officialId = null, Guid? enginId = null, DateTime? startDate = null, int page = 1, int pageSize = 5
        );

        Task<Affectation?> GetByIdAsync(Guid id);
        Task<Affectation> AddAsync(Affectation affectation);
        Task<Affectation?> UpdateAsync(Affectation affectation);
        Task<bool> DeleteAsync(Guid id);
    }
}
