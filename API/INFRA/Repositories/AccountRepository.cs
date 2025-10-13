using PATOA.CORE.Interfaces;
using PATOA.CORE.Entities;
using PATOA.INFRA.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace PATOA.INFRA.Repositories;

public class AccountRepository : GenericRepository<Account>, IAccountRepository
{

    public AccountRepository(ApplicationDbContext context, IUnitOfWork unitOfWork)
            : base(context, unitOfWork)
    {
       
    }

    public async Task<Account> GetByUsername(string username)
    {
        return await _context.Accounts
            .Include(a => a.Role)
            .FirstOrDefaultAsync(a => a.Username.ToLower() == username.ToLower());
    }

    public async Task<Account> GetByEmail(string email)
    {
        return await _context.Accounts
            .Include(a => a.Role)
            .FirstOrDefaultAsync(a => a.Email.ToLower() == email.ToLower());
    }

    public async Task<bool> IsEmailUnique(string email)
    {
        return !await _context.Accounts
            .AnyAsync(a => a.Email.ToLower() == email.ToLower());
    }

    public async Task<bool> IsUsernameUnique(string username)
    {
        return !await _context.Accounts
            .AnyAsync(a => a.Username.ToLower() == username.ToLower());
    }

    public async Task<IEnumerable<Account>> GetAccountsByRole(string role)
    {
        return await _context.Accounts
            .Include(a => a.Role)
            .Where(a => a.Role.Name.ToLower() == role.ToLower())
            .ToListAsync();
    }

    public async Task<bool> ValidateCredentials(string username, string password)
    {
        var account = await GetByUsername(username);
        if (account == null) return false;

        var hashedPassword = HashPassword(password, account.Salt);
        return account.PasswordHash == hashedPassword;
    }

    private string HashPassword(string password, string salt)
    {
        using (var sha256 = SHA256.Create())
        {
            var saltedPassword = string.Concat(password, salt);
            var bytes = Encoding.UTF8.GetBytes(saltedPassword);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
} 