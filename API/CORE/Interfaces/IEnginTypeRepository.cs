using PATOA.CORE.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PATOA.CORE.Interfaces
{
    public interface IEnginTypeRepository
    {
        Task<IEnumerable<EnginType>> GetAllAsync();
        Task<EnginType?> GetByCodeAsync(string code);
        Task<EnginType> AddAsync(EnginType type);
        Task UpdateAsync(EnginType type);
        Task DeleteAsync(string code);
        Task<bool> ExistsAsync(string code);

        Task<(IEnumerable<EnginType> Data, int TotalCount)> GetAllAsync(string? name, int pageNumber, int pageSize);
    }
}
