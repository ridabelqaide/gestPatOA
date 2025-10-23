using PATOA.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PATOA.APPLICATION.Interfaces;

    public interface IPrivatePatService
    {
        Task<IEnumerable<PrivatePat>> GetAllAsync(string? registrationNumber = null);
        Task<PrivatePat?> GetByIdAsync(Guid id);
        Task<PrivatePat> CreateAsync(PrivatePat privatePat);
        Task<bool> UpdateAsync(Guid id, PrivatePat privatePat);
        Task<bool> DeleteAsync(Guid id);
    }
