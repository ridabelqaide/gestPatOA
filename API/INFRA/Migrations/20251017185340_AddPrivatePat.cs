using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INFRA.Migrations
{
    /// <inheritdoc />
    public partial class AddPrivatePat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrivatePats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumeroTaqid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    References = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferenceEnregistrement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Superficie = table.Column<double>(type: "float", nullable: false),
                    Localisation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Coordinates = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SourceAcquisition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrixAchat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Affectation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsageEffectif = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValeurMarchande = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UsageParTiers = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivatePats", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrivatePats");
        }
    }
}
