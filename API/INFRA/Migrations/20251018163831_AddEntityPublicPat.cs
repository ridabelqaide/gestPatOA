using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INFRA.Migrations
{
    /// <inheritdoc />
    public partial class AddEntityPublicPat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RentAmount",
                table: "PrivatePats",
                newName: "RentalPrice");

            migrationBuilder.RenameColumn(
                name: "LeaseDurationMonths",
                table: "PrivatePats",
                newName: "LeaseDuration");

            migrationBuilder.RenameColumn(
                name: "LeaseDurationDescription",
                table: "PrivatePats",
                newName: "ZoningDesignationAr");

            migrationBuilder.RenameColumn(
                name: "LeaseAgreementReference",
                table: "PrivatePats",
                newName: "TypeAr");

            migrationBuilder.AddColumn<string>(
                name: "AcquisitionSourceAr",
                table: "PrivatePats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CurrentUseAr",
                table: "PrivatePats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LandReferencesAr",
                table: "PrivatePats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LocationAr",
                table: "PrivatePats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NotesAr",
                table: "PrivatePats",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrivateDomainRemovalJustificationAr",
                table: "PrivatePats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivateUseDetailsAr",
                table: "PrivatePats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantNameAr",
                table: "PrivatePats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PublicPats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LandReferences = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LandReferencesAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationReference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Area = table.Column<double>(type: "float", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocationAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AcquisitionSource = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AcquisitionSourceAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LegalBasis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LegalBasisAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZoningDesignation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZoningDesignationAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentUse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentUseAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MarketValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsPrivatelyUsedByThirdParty = table.Column<bool>(type: "bit", nullable: false),
                    PrivateUseDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrivateUseDetailsAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OccupationPermitDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AuthorizedPerson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorizedPersonAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemporaryOccupationDuration = table.Column<int>(type: "int", nullable: true),
                    OccupationFeeDuration = table.Column<int>(type: "int", nullable: true),
                    OccupationFeeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IncreaseRatePercent = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaymentDeadlinesAndMethods = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDeadlinesAndMethodsAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RemovalFromPublicDomainReference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RemovalFromPublicDomainDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotesAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicPats", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PublicPats");

            migrationBuilder.DropColumn(
                name: "AcquisitionSourceAr",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "CurrentUseAr",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "LandReferencesAr",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "LocationAr",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "NotesAr",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "PrivateDomainRemovalJustificationAr",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "PrivateUseDetailsAr",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "TenantNameAr",
                table: "PrivatePats");

            migrationBuilder.RenameColumn(
                name: "ZoningDesignationAr",
                table: "PrivatePats",
                newName: "LeaseDurationDescription");

            migrationBuilder.RenameColumn(
                name: "TypeAr",
                table: "PrivatePats",
                newName: "LeaseAgreementReference");

            migrationBuilder.RenameColumn(
                name: "RentalPrice",
                table: "PrivatePats",
                newName: "RentAmount");

            migrationBuilder.RenameColumn(
                name: "LeaseDuration",
                table: "PrivatePats",
                newName: "LeaseDurationMonths");
        }
    }
}
