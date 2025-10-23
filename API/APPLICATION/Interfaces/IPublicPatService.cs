using PATOA.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PATOA.APPLICATION.Interfaces
{
    public interface IPublicPatService
    {
        Task<IEnumerable<PublicPat>> GetAllAsync(string? registrationNumber = null);
        Task<PublicPat?> GetByIdAsync(Guid id);
        Task CreateAsync(PublicPat publicPat);
        Task<bool> UpdateAsync(Guid id, PublicPat publicPat);
        Task<bool> DeleteAsync(Guid id);
    }
}
