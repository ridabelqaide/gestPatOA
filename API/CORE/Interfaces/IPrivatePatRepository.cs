using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PATOA.CORE.Entities;

namespace PATOA.CORE.Interfaces
{
    public interface IPrivatePatRepository
    {
        Task<IEnumerable<PrivatePat>> GetAllAsync();
        IQueryable<PrivatePat> QueryAll();
        Task<PrivatePat?> GetByIdAsync(Guid id);
        Task<PrivatePat> AddAsync(PrivatePat entity);
        Task UpdateAsync(PrivatePat existing, PrivatePat updated);
        Task DeleteAsync(Guid id);
    }
}