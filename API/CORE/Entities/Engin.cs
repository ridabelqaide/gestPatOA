using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace PATOA.CORE.Entities
{
    public class Engin : BaseEntity
    {
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(150)]
        public string Marque { get; set; }
        [MaxLength(10)]
        public string? Model { get; set; }
        public string? PuissanceFiscal { get; set; }
        public DateTime? MiseCirculationDate { get; set; }
        public string? Genre { get; set; }

        public string? EnginTypeCode { get; set; }   
        public EnginType? EnginType { get; set; }

        [MaxLength(50)]
        public string ModeCarburant { get; set; }

        [MaxLength(100)]
        public string? Acquisition { get; set; }

        [MaxLength(50)]
        public string Etat { get; set; }

        [MaxLength(50)]
        public string? Matricule { get; set; }
        public decimal? TH { get; set; }
        public decimal? TJ { get; set; }
        [MaxLength(50)]

        
        // Relation avec les assurances
        public ICollection<Insurance> Insurances { get; set; } = new List<Insurance>();
        [JsonIgnore]
        public ICollection<Affectation> Affectations { get; set; } = new List<Affectation>();
    }

}
