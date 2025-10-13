using System.ComponentModel.DataAnnotations;

namespace PATOA.CORE.Entities
{
    public abstract class BaseBlob : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string FileName { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string ContentType { get; set; }
        
        [Required]
        public long FileSize { get; set; }
        
        [Required]
        public string FilePath { get; set; }
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
    }
} 