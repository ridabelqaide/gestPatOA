using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PATOA.CORE.Entities
{
    public class Official : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string CIN { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        [MaxLength(100)]
        public string? Fonction { get; set; }

        [MaxLength(20)]
        public string? Genre { get; set; }

        public DateTime? DateEmbauche { get; set; }

        [MaxLength(150)]
        public string? Service { get; set; }

        public string? Details { get; set; }

        [JsonIgnore]
        public ICollection<Affectation> Affectations { get; set; } = new List<Affectation>();
    }
}
