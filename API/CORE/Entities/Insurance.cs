using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PATOA.CORE.Entities
{
    public class Insurance : BaseEntity
    {
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(150)]
        public string Company { get; set; }
        [MaxLength(10)]
 
        public DateTime startDate { get; set; }
        public DateTime EndDate { get; set; }

        [MaxLength(250)]
        public string? Type { get; set; }
        public Decimal Amount { get; set; }

        public Guid EnginId { get; set; }
        public Engin Engin { get; set; }

    }
    
}
