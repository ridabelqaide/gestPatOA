using PATOA.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PATOA.CORE.Interfaces
{
    public interface IInsuranceRepository
    {
        Task<IEnumerable<Insurance>> GetAllAsync();
        Task<Insurance?> GetByIdAsync(Guid id);
        Task AddAsync(Insurance insurance);
        Task UpdateAsync(Insurance insurance);
        Task DeleteAsync(Guid id);
    }
}
