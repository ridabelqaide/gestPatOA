using System;
using System.ComponentModel.DataAnnotations;

namespace PATOA.CORE.Entities
    {
    public class PrivatePat : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string RegistrationNumber { get; set; }

        public string Type { get; set; }

        public string TypeAr { get; set; }

        public string LandReferences { get; set; }

        public string LandReferencesAr { get; set; } 

        public string RegistrationReference { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public double? Area { get; set; }

        public string Location { get; set; }

        public string LocationAr { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }


        public string AcquisitionSource { get; set; }
        public string AcquisitionSourceAr { get; set; }

        public decimal? PurchasePrice { get; set; }

        public string ZoningDesignation { get; set; }
        public string ZoningDesignationAr { get; set; }

        public string CurrentUse { get; set; }
        public string CurrentUseAr { get; set; }

        public decimal? MarketValue { get; set; }

        public bool IsPrivatelyUsedByThirdParty { get; set; }
        public string PrivateUseDetails { get; set; }
        public string PrivateUseDetailsAr { get; set; }

        public DateTime? LeaseAgreementDate { get; set; }

        public string TenantName { get; set; }
        public string TenantNameAr { get; set; }

        public int? LeaseDuration { get; set; }
      

        public decimal? RentalPrice { get; set; }
        public decimal? IncreaseRatePercent { get; set; }
        public DateTime? RentPaymentDate { get; set; }

        public string PrivateDomainRemovalReference { get; set; }
        public DateTime? PrivateDomainRemovalDate { get; set; }
        public string PrivateDomainRemovalJustification { get; set; }
        public string PrivateDomainRemovalJustificationAr { get; set; }

        public string TransferOrPublicDomainReference { get; set; }
        public DateTime? TransferOrPublicDomainDate { get; set; }
        public decimal? TransferPriceOrExchangeAmount { get; set; }

        public string? Notes { get; set; }
        public string? NotesAr { get; set; }
    }
}

