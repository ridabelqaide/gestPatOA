using PATOA.APPLICATION.Interfaces;
using PATOA.CORE.Interfaces;
using PATOA.CORE.Entities;
using System.Security.Cryptography;
using System.Text;

namespace PATOA.APPLICATION.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<IEnumerable<Account>> GetAllAccounts()
    {
        return await _accountRepository.GetAllAsync();
    }

    public async Task<Account> GetAccountById(Guid id)
    {
        return await _accountRepository.GetByIdGuidAsync(id);
    }

    public async Task<Account> GetAccountByUsername(string username)
    {
        return await _accountRepository.GetByUsername(username);
    }

    public async Task<Account> GetAccountByEmail(string email)
    {
        return await _accountRepository.GetByEmail(email);
    }

    public async Task<IEnumerable<Account>> GetAccountsByRole(string role)
    {
        return await _accountRepository.GetAccountsByRole(role);
    }

    public async Task<Account> CreateAccount(Account account, string password)
    {
        if (!await IsUsernameAvailable(account.Username))
            throw new InvalidOperationException("Username is already taken");

        if (!await IsEmailAvailable(account.Email))
            throw new InvalidOperationException("Email is already in use");

        account.SetPassword(password);
        return await _accountRepository.AddAsync(account);
    }

    public async Task<Account> UpdateAccount(Account account)
    {
        var existingAccount = await _accountRepository.GetByIdGuidAsync(account.Id);
        if (existingAccount == null)
            throw new KeyNotFoundException("Account not found");

        if (existingAccount.Username != account.Username && !await IsUsernameAvailable(account.Username))
            throw new InvalidOperationException("Username is already taken");

        if (existingAccount.Email != account.Email && !await IsEmailAvailable(account.Email))
            throw new InvalidOperationException("Email is already in use");

        // Don't update password-related fields through this method
        account.UpdatedOn = DateTime.UtcNow;
        await _accountRepository.UpdateAsync(account);
        return account;
    }

    public async Task<bool> DeleteAccount(Guid id)
    {
        var account = await _accountRepository.GetByIdGuidAsync(id);
        if (account == null)
            return false;

        try
        {
            await _accountRepository.DeleteAsync(account);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> ChangePassword(Guid accountId, string currentPassword, string newPassword)
    {
        var account = await _accountRepository.GetByIdGuidAsync(accountId);
        if (account == null)
            return false;

        if (!account.VerifyPassword(currentPassword))
            return false;

        account.SetPassword(newPassword);
        await _accountRepository.UpdateAsync(account);
        return true;
    }

    public async Task<bool> ResetPassword(string email)
    {
        var account = await _accountRepository.GetByEmail(email);
        if (account == null)
            return false;

        var newPassword = GenerateRandomPassword();
        account.SetPassword(newPassword);
        await _accountRepository.UpdateAsync(account);

        // TODO: Send email with new password
        return true;
    }

    public async Task<bool> ValidateCredentials(string username, string password)
    {
        var account = await _accountRepository.GetByUsername(username);
        if (account == null)
            return false;

        if (account.IsLocked)
            return false;

        if (!account.VerifyPassword(password))
        {
            account.IncrementLoginAttempts();
            await _accountRepository.UpdateAsync(account);
            return false;
        }

        account.ResetLoginAttempts();
        await _accountRepository.UpdateAsync(account);
        return true;
    }

    public async Task<bool> IsUsernameAvailable(string username)
    {
        return await _accountRepository.IsUsernameUnique(username);
    }

    public async Task<bool> IsEmailAvailable(string email)
    {
        return await _accountRepository.IsEmailUnique(email);
    }

    public async Task<bool> LockAccount(Guid id)
    {
        var account = await _accountRepository.GetByIdGuidAsync(id);
        if (account == null)
            return false;

        account.Lock();
        await _accountRepository.UpdateAsync(account);
        return true;
    }

    public async Task<bool> UnlockAccount(Guid id)
    {
        var account = await _accountRepository.GetByIdGuidAsync(id);
        if (account == null)
            return false;

        account.Unlock();
        await _accountRepository.UpdateAsync(account);
        return true;
    }

    private string GenerateRandomPassword()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 12)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
} 