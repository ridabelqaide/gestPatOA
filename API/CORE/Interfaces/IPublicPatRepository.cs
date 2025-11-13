using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PATOA.CORE.Entities;

namespace PATOA.CORE.Interfaces
{
    public interface IPublicPatRepository
    {
        Task<IEnumerable<PublicPat?>> GetAllAsync();
        IQueryable<PublicPat> QueryAll();
        Task<PublicPat?> GetByIdAsync(Guid id);
        Task CreateAsync(PublicPat entity);
        Task UpdateAsync(PublicPat existing, PublicPat updated);
        Task DeleteAsync(PublicPat entity);
        
    }
}