using System;

namespace PATOA.CORE.Entities
{
    public abstract class BaseEntity
    {
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } 
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
} 