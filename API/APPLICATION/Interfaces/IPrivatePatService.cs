using PATOA.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PATOA.APPLICATION.DTOs;

namespace PATOA.APPLICATION.Interfaces;

    public interface IPrivatePatService
    {
        Task<IEnumerable<PrivatePat>> GetAllAsync();
        Task<PagedResult<PrivatePat>> GetPagedAsync(
           string? type = null,
           string? location = null,
           string? registrationNumber = null,
           int page = 1,
           int pageSize = 10);
        Task<PrivatePat?> GetByIdAsync(Guid id);
        Task<PrivatePat> CreateAsync(PrivatePat privatePat);
        Task<bool> UpdateAsync(Guid id, PrivatePat privatePat);
        Task<bool> DeleteAsync(Guid id);
    }
