using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INFRA.Migrations
{
    /// <inheritdoc />
    public partial class addEnginType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Engins");

            migrationBuilder.AddColumn<string>(
                name: "EnginTypeCode",
                table: "Engins",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "EnginType",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnginType", x => x.Code);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Engins_EnginTypeCode",
                table: "Engins",
                column: "EnginTypeCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Engins_EnginType_EnginTypeCode",
                table: "Engins",
                column: "EnginTypeCode",
                principalTable: "EnginType",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Engins_EnginType_EnginTypeCode",
                table: "Engins");

            migrationBuilder.DropTable(
                name: "EnginType");

            migrationBuilder.DropIndex(
                name: "IX_Engins_EnginTypeCode",
                table: "Engins");

            migrationBuilder.DropColumn(
                name: "EnginTypeCode",
                table: "Engins");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Engins",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
