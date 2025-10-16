using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public DateTime MiseCirculationDate { get; set; }
        public decimal TH { get; set; }
        public decimal TJ { get; set; }
        [MaxLength(50)]
        public string Genre { get; set; }

        [MaxLength(50)]
        public string Type { get; set; }

        [MaxLength(50)]
        public string ModeCarburant { get; set; }

        [MaxLength(100)]
        public string Acquisition { get; set; }

        [MaxLength(50)]
        public string Etat { get; set; }

        [MaxLength(50)]
        public string Matricule { get; set; }
        
        // Relation avec les assurances
        public ICollection<Insurance> Insurances { get; set; } = new List<Insurance>();
    }
    
}
