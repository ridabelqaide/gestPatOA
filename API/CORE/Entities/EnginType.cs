using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PATOA.CORE.Entities
{
    public class EnginType : BaseEntity
    {
        [Key]
        [MaxLength(50)]
        public string Code { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }

        public ICollection<Engin> Engins { get; set; } = new List<Engin>();
    }
}
