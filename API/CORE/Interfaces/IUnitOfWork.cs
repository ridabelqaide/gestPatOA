using System;
using System.Threading.Tasks;

namespace PATOA.CORE.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
} 