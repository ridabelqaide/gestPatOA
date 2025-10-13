using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PATOA.CORE.Entities
{
    public class Right : BaseEntity
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        
        public string? Description { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; } = null!;
        public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}
