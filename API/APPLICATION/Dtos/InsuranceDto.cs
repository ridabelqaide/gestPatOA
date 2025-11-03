using System;

namespace PATOA.APPLICATION.DTOs
{
    public class InsuranceDto
    {
        public Guid Id { get; set; }
        public string Company { get; set; }
        public string? Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime startDate { get; set; }
        public DateTime EndDate { get; set; }

        public Guid EnginId { get; set; }
        public string? Matricule { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

