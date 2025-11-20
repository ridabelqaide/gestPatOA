using PATOA.CORE.Entities;
using PATOA.APPLICATION.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PATOA.APPLICATION.Interfaces
{
    public interface IEnginTypeService
    {
        Task<IEnumerable<EnginType>> GetAllAsync();
        Task<PagedResult<EnginType>> GetPagedAsync(string? name, int pageNumber, int pageSize);
        Task<EnginType?> GetByCodeAsync(string code);
        Task<EnginType> CreateAsync(EnginType model);
        Task<EnginType?> UpdateAsync(string code, EnginType model);
        Task<bool> DeleteAsync(string code);
    }
}
