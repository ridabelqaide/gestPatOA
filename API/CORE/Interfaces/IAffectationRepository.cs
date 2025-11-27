using PATOA.CORE.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PATOA.CORE.Interfaces
{
    public interface IAffectationRepository
    {
        IQueryable<Affectation> GetAllQueryable();
        Task<Affectation?> GetByIdAsync(Guid id);
        Task<Affectation> AddAsync(Affectation affectation);
        Task UpdateAsync(Affectation affectation);
        Task DeleteAsync(Guid id);
    }
}
