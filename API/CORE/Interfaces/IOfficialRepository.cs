using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PATOA.CORE.Entities;
namespace PATOA.CORE.Interfaces
{
    public interface IOfficialRepository
    {
        Task<IEnumerable<Official>> GetAllAsync();
        IQueryable<Official> GetAllQueryable();
        Task<Official?> GetByIdAsync(Guid id);
        Task<Official> AddAsync(Official official);
        Task UpdateAsync(Guid id, Official official);
        Task DeleteAsync(Guid id);
    }
}