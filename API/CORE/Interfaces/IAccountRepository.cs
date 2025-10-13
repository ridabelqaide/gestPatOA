using PATOA.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PATOA.CORE.Interfaces;

public interface IAccountRepository : IGenericRepository<Account>
{
    Task<Account> GetByUsername(string username);
    Task<Account> GetByEmail(string email);
    Task<bool> IsEmailUnique(string email);
    Task<bool> IsUsernameUnique(string username);
    Task<IEnumerable<Account>> GetAccountsByRole(string role);
    Task<bool> ValidateCredentials(string username, string password);
} 