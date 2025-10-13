using System;
using System.ComponentModel.DataAnnotations;

namespace PATOA.APPLICATION.DTOs.AdminDTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public int LoginAttempts { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }

    public class CreateUserDto
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string Password { get; set; } = string.Empty;
        
        [Required]
        public int RoleId { get; set; }
        
        public bool IsActive { get; set; } = true;
    }

    public class UpdateUserDto
    {
        [MaxLength(50)]
        public string? Username { get; set; }
        
        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }
        
        public string? Password { get; set; }
        
        public int? RoleId { get; set; }
        
        public bool? IsActive { get; set; }
        
        public bool? IsLocked { get; set; }
    }
}
