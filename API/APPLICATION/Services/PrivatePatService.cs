using PATOA.APPLICATION.Interfaces;
using PATOA.CORE.Interfaces;
using PATOA.CORE.Entities;
using System.Security.Cryptography;
using PATOA.CORE.Interfaces;
using System.Text;


namespace PATOA.APPLICATION.Services
{
    public class PrivatePatService : IPrivatePatService
    {
        private readonly IPrivatePatRepository _repository;

        public PrivatePatService(IPrivatePatRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PrivatePat>> GetAllAsync(string? registrationNumber = null)
        {
            return await _repository.GetAllAsync(registrationNumber);
        }

        public async Task<PrivatePat?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<PrivatePat> CreateAsync(PrivatePat privatePat)
        {
            privatePat.CreatedOn = DateTime.UtcNow;
            return await _repository.AddAsync(privatePat);
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