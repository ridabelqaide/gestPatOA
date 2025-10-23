using PATOA.CORE.Entities;
using PATOA.CORE.Interfaces;
using PATOA.APPLICATION.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<PublicPat>> GetAllAsync(string? registrationNumber = null)
        {
            return await _repository.GetAllAsync(registrationNumber);
        }

        public async Task CreateAsync(PublicPat publicPat)
        {
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
