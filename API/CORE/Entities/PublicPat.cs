using System;
using System.ComponentModel.DataAnnotations;

namespace PATOA.CORE.Entities
{
    public class PublicPat : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string? RegistrationNumber { get; set; }

        public string? Type { get; set; }
        public string? TypeAr { get; set; }

        public string? LandReferences { get; set; }
        public string? LandReferencesAr { get; set; }

        public string? RegistrationReference { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public double? Area { get; set; } 

        public string? Location { get; set; }
        public string? LocationAr { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public string? AcquisitionSource { get; set; }
        public string? AcquisitionSourceAr { get; set; }

        public decimal? PurchasePrice { get; set; } 

        public string? LegalBasis { get; set; }
        public string? LegalBasisAr { get; set; } 

        public string? ZoningDesignation { get; set; }
        public string? ZoningDesignationAr { get; set; }

        public string? CurrentUse { get; set; }
        public string? CurrentUseAr { get; set; }

        public decimal? MarketValue { get; set; } 

        public bool IsPrivatelyUsedByThirdParty { get; set; }
        public string? PrivateUseDetails { get; set; }
        public string? PrivateUseDetailsAr { get; set; }

        public DateTime? OccupationPermitDate { get; set; }

        public string? AuthorizedPerson { get; set; }
        public string? AuthorizedPersonAr { get; set; }

        public int? TemporaryOccupationDuration { get; set; } 
        public int? OccupationFeeDuration { get; set; } 

        public decimal? OccupationFeeAmount { get; set; } 
        public decimal? IncreaseRatePercent { get; set; } 

        public string? PaymentDeadlinesAndMethods { get; set; }
        public string? PaymentDeadlinesAndMethodsAr { get; set; }

        public string? RemovalFromPublicDomainReference { get; set; }
        public DateTime? RemovalFromPublicDomainDate { get; set; }

        public string? Notes { get; set; }
        public string? NotesAr { get; set; } = string.Empty;
    }
}
