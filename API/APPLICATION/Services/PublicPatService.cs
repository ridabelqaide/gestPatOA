using PATOA.CORE.Entities;
using PATOA.CORE.Interfaces;
using PATOA.APPLICATION.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PATOA.APPLICATION.DTOs;
using Microsoft.EntityFrameworkCore;

namespace PATOA.APPLICATION.Services
{
    public class PublicPatService : IPublicPatService
    {
        private readonly IPublicPatRepository _repository;

        public PublicPatService(IPublicPatRepository repository)
        {
            _repository = repository;
        }

        public async Task<PublicPat?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<PagedResult<PublicPat>> GetPagedAsync(
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

            return new PagedResult<PublicPat>
            {
                Data = data,
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<IEnumerable<PublicPat>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task CreateAsync(PublicPat publicPat)
        {
            var exists = await _repository.GetAllAsync();

            if (exists.Any(p => p.RegistrationNumber == publicPat.RegistrationNumber))
            {
                throw new Exception("registerNum existe déjà.");
            }
            publicPat.Id = Guid.NewGuid();
            await _repository.CreateAsync(publicPat);
        }

        public async Task<bool> UpdateAsync(Guid id, PublicPat publicPat)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            publicPat.Id = existing.Id;

            await _repository.UpdateAsync(existing, publicPat);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            await _repository.DeleteAsync(existing);
            return true;
        }
    }
}
