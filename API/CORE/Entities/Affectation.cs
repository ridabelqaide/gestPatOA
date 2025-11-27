using System;
using System.ComponentModel.DataAnnotations;

namespace PATOA.CORE.Entities
{
    public class Affectation : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public double? CurrentKm { get; set; }

        public double? EndKm { get; set; }

        public string? Object { get; set; }

        [MaxLength(500)]
        public string? Details { get; set; }

        public Guid? EnginId { get; set; }
        public Engin? Engin { get; set; }

        public Guid? OfficialId { get; set; }
        public Official? Official { get; set; }

    }
}
