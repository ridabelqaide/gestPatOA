using PATOA.CORE.Entities;
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using PATOA.CORE.Interfaces;
using Microsoft.Extensions.Configuration;

namespace PATOA.APPLICATION.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IAccountRepository accountRepository, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _configuration = configuration;
        }

        public async Task<LoginResponse> LoginAsync(string username, string password)
        {
            var account = await _accountRepository.GetByUsername(username);
            
            if (account == null || account.IsLocked || !account.VerifyPassword(password))
            {
                if (account != null)
                {
                    account.IncrementLoginAttempts();
                    await _accountRepository.UpdateAsync(account);
                }
                return null;
            }

            // Reset login attempts on successful login
            account.ResetLoginAttempts();
            await _accountRepository.UpdateAsync(account);

            // Generate JWT token
            var token = GenerateJwtToken(account, Convert.ToDouble(_configuration["Jwt:AdminToken:ExpirationMinutes"]), _configuration["Jwt:AdminToken:SecretKey"]);
            var expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:AdminToken:ExpirationMinutes"]));

            return new LoginResponse
            {
                Token = token,
                Username = account.Username,
                Email = account.Email,
                Role = account.Role?.Name ?? "User",
                Expiration = expiration
            };
        }
        public async Task<LoginResponse> LoginBCAsync(string username, string password)
        {
            var account = await _accountRepository.GetByUsername(username);

            if (account == null || account.IsLocked || !account.VerifyPassword(password))
            {
                if (account != null)
                {
                    account.IncrementLoginAttempts();
                    await _accountRepository.UpdateAsync(account);
                }
                return null;
            }

            // Reset login attempts on successful login
            account.ResetLoginAttempts();
            await _accountRepository.UpdateAsync(account);

            // Generate JWT token
            var token = GenerateJwtToken(account, Convert.ToDouble(_configuration["Jwt:PublicToken:ExpirationMinutes"]), _configuration["Jwt:PublicToken:SecretKey"]);
            var expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:PublicToken:ExpirationMinutes"]));

            return new LoginResponse
            {
                Token = token,
                Username = account.Username,
                Email = account.Email,
                Role = account.Role?.Name ?? "User",
                Expiration = expiration
            };
        }

        public async Task<Account> RegisterAsync(Account account, string password)
        {
            if (await _accountRepository.GetByUsername(account.Username) != null)
            {
                return null;
            }

            if (await _accountRepository.GetByEmail(account.Email) != null)
            {
                return null;
            }

            account.SetPassword(password);
            await _accountRepository.AddAsync(account);

            return account;
        }

        public async Task<bool> ChangePasswordAsync(Guid accountId, string currentPassword, string newPassword)
        {
            var account = await _accountRepository.GetByIdGuidAsync(accountId);
            if (account == null || !account.VerifyPassword(currentPassword))
                return false;

            account.SetPassword(newPassword);
            await _accountRepository.UpdateAsync(account);
            return true;
        }

        public async Task<Account> GetAccountByIdAsync(Guid id)
        {
            return await _accountRepository.GetByIdGuidAsync(id);
        }

        public Task<bool> ValidateTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
                return Task.FromResult(false);

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        public async Task<bool> IsInRoleAsync(Guid accountId, string roleName)
        {
            var account = await _accountRepository.GetByIdGuidAsync(accountId);
            return account?.Role?.Name == roleName;
        }

        public async Task<bool> UpdateAccountAsync(Account account)
        {
            if (account == null)
                return false;

            await _accountRepository.UpdateAsync(account);
            return true;
        }

        public async Task<bool> DeactivateAccountAsync(Guid accountId)
        {
            var account = await _accountRepository.GetByIdGuidAsync(accountId);
            if (account == null)
                return false;

            account.IsActive = false;
            await _accountRepository.UpdateAsync(account);
            return true;
        }

        public async Task<bool> ReactivateAccountAsync(Guid accountId)
        {
            var account = await _accountRepository.GetByIdGuidAsync(accountId);
            if (account == null)
                return false;

            account.IsActive = true;
            await _accountRepository.UpdateAsync(account);
            return true;
        }

        private string GenerateJwtToken(Account account,double expire,string key)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, account.Username),
                new Claim(JwtRegisteredClaimNames.Email, account.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, account.Role?.Name ?? "User")
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(expire)),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
