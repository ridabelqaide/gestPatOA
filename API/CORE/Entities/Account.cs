using System;
using System.Security.Cryptography;
using System.Text;

namespace PATOA.CORE.Entities
{
    public class Account : BaseEntity
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public string Salt { get; private set; } = string.Empty;
        public bool IsLocked { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public int LoginAttempts { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; } = null!;
        public DateTime? LastLogin { get; set; }
        
        // Private constructor for EF Core
        private Account() { }

        // Public constructor for creating new accounts
        public Account(string username, string email)
        {
            Username = username;
            Email = email;
            IsLocked = false;
            LoginAttempts = 0;
        }

        // Method to set password with automatic salt generation
        public void SetPassword(string password)
        {
            Salt = GenerateSalt();
            PasswordHash = HashPassword(password);
        }

        // Method to verify a password
        public bool VerifyPassword(string password)
        {
            return PasswordHash == HashPassword(password);
        }

        // Method to increment failed login attempts
        public void IncrementLoginAttempts()
        {
            LoginAttempts++;
            if (LoginAttempts >= 5) // You can adjust this threshold
            {
                IsLocked = true;
            }
            UpdatedOn = DateTime.UtcNow;
        }

        // Method to reset login attempts
        public void ResetLoginAttempts()
        {
            LoginAttempts = 0;
            LastLoginDate = DateTime.UtcNow;
            UpdatedOn = DateTime.UtcNow;
        }

        // Method to lock the account
        public void Lock()
        {
            IsLocked = true;
            UpdatedOn = DateTime.UtcNow;
        }

        // Method to unlock the account
        public void Unlock()
        {
            IsLocked = false;
            LoginAttempts = 0;
            UpdatedOn = DateTime.UtcNow;
        }

        // Private method to generate a random salt
        private string GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        // Private method to hash the password with the salt
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = string.Concat(password, Salt);
                var bytes = Encoding.UTF8.GetBytes(saltedPassword);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
