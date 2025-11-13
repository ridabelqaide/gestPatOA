using PATOA.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PATOA.APPLICATION.DTOs;
namespace PATOA.APPLICATION.Interfaces
{
    public interface IPublicPatService
    {
        Task<IEnumerable<PublicPat>> GetAllAsync();
        Task<PagedResult<PublicPat>> GetPagedAsync(
          string? type = null,
          string? location = null,
          string? registrationNumber = null,
          int page = 1,
          int pageSize = 10);
        Task<PublicPat?> GetByIdAsync(Guid id);
        Task CreateAsync(PublicPat publicPat);
        Task<bool> UpdateAsync(Guid id, PublicPat publicPat);
        Task<bool> DeleteAsync(Guid id);
    }
}
