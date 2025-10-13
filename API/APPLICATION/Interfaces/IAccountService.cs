using PATOA.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PATOA.APPLICATION.Interfaces;

public interface IAccountService
{
    Task<IEnumerable<Account>> GetAllAccounts();
    Task<Account> GetAccountById(Guid id);
    Task<Account> GetAccountByUsername(string username);
    Task<Account> GetAccountByEmail(string email);
    Task<IEnumerable<Account>> GetAccountsByRole(string role);
    Task<Account> CreateAccount(Account account, string password);
    Task<Account> UpdateAccount(Account account);
    Task<bool> DeleteAccount(Guid id);
    Task<bool> ChangePassword(Guid accountId, string currentPassword, string newPassword);
    Task<bool> ResetPassword(string email);
    Task<bool> ValidateCredentials(string username, string password);
    Task<bool> IsUsernameAvailable(string username);
    Task<bool> IsEmailAvailable(string email);
    Task<bool> LockAccount(Guid id);
    Task<bool> UnlockAccount(Guid id);
} 