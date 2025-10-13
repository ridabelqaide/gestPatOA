using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PATOA.CORE.Entities
{
    public class Role : BaseEntity
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        
        public string? Description { get; set; }
        
        public ICollection<Account> Accounts { get; set; } = new List<Account>();
        public ICollection<Right> Rights { get; set; } = new List<Right>();
    }
}
