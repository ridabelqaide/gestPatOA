using PATOA.APPLICATION.Interfaces;
using PATOA.CORE.Interfaces;
using PATOA.CORE.Entities;
using System.Security.Cryptography;
using PATOA.CORE.Interfaces;
using System.Text;
using PATOA.APPLICATION.DTOs;
using Microsoft.EntityFrameworkCore;

namespace PATOA.APPLICATION.Services
{
    public class PrivatePatService : IPrivatePatService
    {
        private readonly IPrivatePatRepository _repository;

        public PrivatePatService(IPrivatePatRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PrivatePat>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<PrivatePat?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<PrivatePat> CreateAsync(PrivatePat privatePat)
        {
            var exists = await _repository.GetAllAsync();
            if (exists.Any(p => p.RegistrationNumber == privatePat.RegistrationNumber))
            {
                throw new Exception("registerNum existe déjà.");
            }

            privatePat.CreatedOn = DateTime.UtcNow;
            return await _repository.AddAsync(privatePat);
        }

        public async Task<PagedResult<PrivatePat>> GetPagedAsync(
      string? typeAr = null,
      string? locationAr = null,
      string? registrationNumber = null,
      int page = 1,
      int pageSize = 10)
        {
            page = page <= 0 ? 1 : page;
            pageSize = pageSize <= 0 ? 10 : pageSize;

            var query = _repository.QueryAll();

            if (!string.IsNullOrEmpty(typeAr))
                query = query.Where(p => p.TypeAr.Contains(typeAr));
            if (!string.IsNullOrEmpty(locationAr))
                query = query.Where(p => p.LocationAr.Contains(locationAr));
            if (!string.IsNullOrEmpty(registrationNumber))
                query = query.Where(p => p.RegistrationNumber.Contains(registrationNumber));

            var totalItems = await query.CountAsync();

            var data = await query
                .OrderByDescending(p => p.CreatedOn)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<PrivatePat>
            {
                Data = data,
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<bool> UpdateAsync(Guid id, PrivatePat privatePat)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return false;

            privatePat.UpdatedOn = DateTime.UtcNow;

            await _repository.UpdateAsync(existing, privatePat);

            return true;
        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            await _repository.DeleteAsync(id);
            return true;
        }
    }
}