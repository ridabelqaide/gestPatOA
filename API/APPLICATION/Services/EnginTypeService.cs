using PATOA.APPLICATION.DTOs;
using PATOA.APPLICATION.Interfaces;
using PATOA.CORE.Entities;
using PATOA.CORE.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PATOA.APPLICATION.Services
{
    public class EnginTypeService : IEnginTypeService
    {
        private readonly IEnginTypeRepository _repo;

        public EnginTypeService(IEnginTypeRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<EnginType>> GetAllAsync()
        {
            return _repo.GetAllAsync();
        }

        public async Task<PagedResult<EnginType>> GetPagedAsync(string? name, int pageNumber, int pageSize)
        {
            var (data, totalCount) = await _repo.GetAllAsync(name, pageNumber, pageSize);

            return new PagedResult<EnginType>
            {
                Data = data.ToList(),
                TotalItems = totalCount,
                Page = pageNumber,
                PageSize = pageSize
            };
        }

        public Task<EnginType?> GetByCodeAsync(string code)
        {
            return _repo.GetByCodeAsync(code);
        }

        public async Task<EnginType> CreateAsync(EnginType model)
        {
            if (await _repo.ExistsAsync(model.Code))
                throw new Exception("Le code existe déjà.");

            return await _repo.AddAsync(model);
        }

        public async Task<EnginType?> UpdateAsync(string code, EnginType model)
        {
            var existing = await _repo.GetByCodeAsync(code);
            if (existing == null) return null;

            existing.Name = model.Name;
            existing.Description = model.Description;

            await _repo.UpdateAsync(existing);
            return existing;
        }

        public async Task<bool> DeleteAsync(string code)
        {
            if (!await _repo.ExistsAsync(code)) return false;

            await _repo.DeleteAsync(code);
            return true;
        }
    }
}
