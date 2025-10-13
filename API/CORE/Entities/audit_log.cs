using System.ComponentModel.DataAnnotations;

namespace PATOA.CORE.Entities
{
    public class AuditLog
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string TableName { get; set; }
        
        [Required]
        public Guid RecordId { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string Action { get; set; }
        
        public string? OldValues { get; set; }
        
        public string? NewValues { get; set; }
        
        [MaxLength(100)]
        public string? UserId { get; set; }
        
        [MaxLength(45)]
        public string? IpAddress { get; set; }
        
        public string? UserAgent { get; set; }
        
        public DateTimeOffset CreatedAt { get; set; }
    }
}