using PATOA.APPLICATION.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PATOA.APPLICATION.Interfaces
{
    public interface IInsuranceService
    {
        Task<IEnumerable<InsuranceDto>> GetAllAsync();
        Task<InsuranceDto?> GetByIdAsync(Guid id);
        Task AddAsync(InsuranceDto dto);
        Task UpdateAsync(Guid id, InsuranceDto dto);
        Task DeleteAsync(Guid id);
    }
}