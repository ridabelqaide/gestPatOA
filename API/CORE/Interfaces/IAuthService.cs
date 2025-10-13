using PATOA.CORE.Entities;
using System.Threading.Tasks;

namespace PATOA.CORE.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(string username, string password);
        Task<LoginResponse> LoginBCAsync(string username, string password);
        Task<Account> RegisterAsync(Account account, string password);
        Task<bool> ChangePasswordAsync(Guid accountId, string currentPassword, string newPassword);
        Task<Account> GetAccountByIdAsync(Guid id);
        Task<bool> ValidateTokenAsync(string token);
        Task<bool> IsInRoleAsync(Guid accountId, string roleName);
        Task<bool> UpdateAccountAsync(Account account);
        Task<bool> DeactivateAccountAsync(Guid accountId);
        Task<bool> ReactivateAccountAsync(Guid accountId);
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime Expiration { get; set; }
    }
} 