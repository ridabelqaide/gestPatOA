using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PATOA.APPLICATION.DTOs.AdminDTOs
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<string> Permissions { get; set; } = new List<string>();
    }

    public class CreateRoleDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        public List<string> Permissions { get; set; } = new List<string>();
    }

    public class UpdateRoleDto
    {
        [MaxLength(50)]
        public string? Name { get; set; }
        
        public string? Description { get; set; }
        
        public List<string>? Permissions { get; set; }
    }
}
