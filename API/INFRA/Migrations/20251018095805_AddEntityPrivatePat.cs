using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INFRA.Migrations
{
    /// <inheritdoc />
    public partial class AddEntityPrivatePat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrixAchat",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "Superficie",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "ValeurMarchande",
                table: "PrivatePats");

            migrationBuilder.RenameColumn(
                name: "UsageParTiers",
                table: "PrivatePats",
                newName: "ZoningDesignation");

            migrationBuilder.RenameColumn(
                name: "UsageEffectif",
                table: "PrivatePats",
                newName: "TransferOrPublicDomainReference");

            migrationBuilder.RenameColumn(
                name: "SourceAcquisition",
                table: "PrivatePats",
                newName: "TenantName");

            migrationBuilder.RenameColumn(
                name: "References",
                table: "PrivatePats",
                newName: "RegistrationReference");

            migrationBuilder.RenameColumn(
                name: "ReferenceEnregistrement",
                table: "PrivatePats",
                newName: "RegistrationNumber");

            migrationBuilder.RenameColumn(
                name: "NumeroTaqid",
                table: "PrivatePats",
                newName: "PrivateUseDetails");

            migrationBuilder.RenameColumn(
                name: "Localisation",
                table: "PrivatePats",
                newName: "PrivateDomainRemovalReference");

            migrationBuilder.RenameColumn(
                name: "Coordinates",
                table: "PrivatePats",
                newName: "PrivateDomainRemovalJustification");

            migrationBuilder.RenameColumn(
                name: "Affectation",
                table: "PrivatePats",
                newName: "Location");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "PrivatePats",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "AcquisitionSource",
                table: "PrivatePats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Area",
                table: "PrivatePats",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "PrivatePats",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "PrivatePats",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<string>(
                name: "CurrentUse",
                table: "PrivatePats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "IncreaseRatePercent",
                table: "PrivatePats",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "PrivatePats",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PrivatePats",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrivatelyUsedByThirdParty",
                table: "PrivatePats",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LandReferences",
                table: "PrivatePats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "PrivatePats",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LeaseAgreementDate",
                table: "PrivatePats",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LeaseAgreementReference",
                table: "PrivatePats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LeaseDurationDescription",
                table: "PrivatePats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "LeaseDurationMonths",
                table: "PrivatePats",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "PrivatePats",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MarketValue",
                table: "PrivatePats",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PrivateDomainRemovalDate",
                table: "PrivatePats",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PurchasePrice",
                table: "PrivatePats",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDate",
                table: "PrivatePats",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RentAmount",
                table: "PrivatePats",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RentPaymentDate",
                table: "PrivatePats",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TransferOrPublicDomainDate",
                table: "PrivatePats",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TransferPriceOrExchangeAmount",
                table: "PrivatePats",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "PrivatePats",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "PrivatePats",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcquisitionSource",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "Area",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "CurrentUse",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "IncreaseRatePercent",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "IsPrivatelyUsedByThirdParty",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "LandReferences",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "LeaseAgreementDate",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "LeaseAgreementReference",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "LeaseDurationDescription",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "LeaseDurationMonths",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "MarketValue",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "PrivateDomainRemovalDate",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "PurchasePrice",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "RegistrationDate",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "RentAmount",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "RentPaymentDate",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "TransferOrPublicDomainDate",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "TransferPriceOrExchangeAmount",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "PrivatePats");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "PrivatePats");

            migrationBuilder.RenameColumn(
                name: "ZoningDesignation",
                table: "PrivatePats",
                newName: "UsageParTiers");

            migrationBuilder.RenameColumn(
                name: "TransferOrPublicDomainReference",
                table: "PrivatePats",
                newName: "UsageEffectif");

            migrationBuilder.RenameColumn(
                name: "TenantName",
                table: "PrivatePats",
                newName: "SourceAcquisition");

            migrationBuilder.RenameColumn(
                name: "RegistrationReference",
                table: "PrivatePats",
                newName: "References");

            migrationBuilder.RenameColumn(
                name: "RegistrationNumber",
                table: "PrivatePats",
                newName: "ReferenceEnregistrement");

            migrationBuilder.RenameColumn(
                name: "PrivateUseDetails",
                table: "PrivatePats",
                newName: "NumeroTaqid");

            migrationBuilder.RenameColumn(
                name: "PrivateDomainRemovalReference",
                table: "PrivatePats",
                newName: "Localisation");

            migrationBuilder.RenameColumn(
                name: "PrivateDomainRemovalJustification",
                table: "PrivatePats",
                newName: "Coordinates");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "PrivatePats",
                newName: "Affectation");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "PrivatePats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PrixAchat",
                table: "PrivatePats",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "Superficie",
                table: "PrivatePats",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<decimal>(
                name: "ValeurMarchande",
                table: "PrivatePats",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
