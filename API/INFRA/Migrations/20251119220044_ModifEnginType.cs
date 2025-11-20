using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INFRA.Migrations
{
    /// <inheritdoc />
    public partial class ModifEnginType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Engins_EnginType_EnginTypeCode",
                table: "Engins");

            migrationBuilder.DropIndex(
                name: "IX_Engins_Matricule",
                table: "Engins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EnginType",
                table: "EnginType");

            migrationBuilder.RenameTable(
                name: "EnginType",
                newName: "EnginTypes");

            migrationBuilder.AlterColumn<decimal>(
                name: "TJ",
                table: "Engins",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TH",
                table: "Engins",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "MiseCirculationDate",
                table: "Engins",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Matricule",
                table: "Engins",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Genre",
                table: "Engins",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "EnginTypeCode",
                table: "Engins",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "Acquisition",
                table: "Engins",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EnginTypes",
                table: "EnginTypes",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Engins_Matricule",
                table: "Engins",
                column: "Matricule",
                unique: true,
                filter: "[Matricule] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Engins_EnginTypes_EnginTypeCode",
                table: "Engins",
                column: "EnginTypeCode",
                principalTable: "EnginTypes",
                principalColumn: "Code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Engins_EnginTypes_EnginTypeCode",
                table: "Engins");

            migrationBuilder.DropIndex(
                name: "IX_Engins_Matricule",
                table: "Engins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EnginTypes",
                table: "EnginTypes");

            migrationBuilder.RenameTable(
                name: "EnginTypes",
                newName: "EnginType");

            migrationBuilder.AlterColumn<decimal>(
                name: "TJ",
                table: "Engins",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TH",
                table: "Engins",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "MiseCirculationDate",
                table: "Engins",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Matricule",
                table: "Engins",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Genre",
                table: "Engins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EnginTypeCode",
                table: "Engins",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Acquisition",
                table: "Engins",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EnginType",
                table: "EnginType",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Engins_Matricule",
                table: "Engins",
                column: "Matricule",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Engins_EnginType_EnginTypeCode",
                table: "Engins",
                column: "EnginTypeCode",
                principalTable: "EnginType",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
