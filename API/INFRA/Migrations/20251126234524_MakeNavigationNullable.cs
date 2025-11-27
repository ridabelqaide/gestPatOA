using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INFRA.Migrations
{
    /// <inheritdoc />
    public partial class MakeNavigationNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Affectation_Engins_EnginId",
                table: "Affectation");

            migrationBuilder.DropForeignKey(
                name: "FK_Affectation_Officials_OfficialId",
                table: "Affectation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Affectation",
                table: "Affectation");

            migrationBuilder.RenameTable(
                name: "Affectation",
                newName: "Affectations");

            migrationBuilder.RenameIndex(
                name: "IX_Affectation_OfficialId",
                table: "Affectations",
                newName: "IX_Affectations_OfficialId");

            migrationBuilder.RenameIndex(
                name: "IX_Affectation_EnginId",
                table: "Affectations",
                newName: "IX_Affectations_EnginId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Affectations",
                table: "Affectations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Affectations_Engins_EnginId",
                table: "Affectations",
                column: "EnginId",
                principalTable: "Engins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Affectations_Officials_OfficialId",
                table: "Affectations",
                column: "OfficialId",
                principalTable: "Officials",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Affectations_Engins_EnginId",
                table: "Affectations");

            migrationBuilder.DropForeignKey(
                name: "FK_Affectations_Officials_OfficialId",
                table: "Affectations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Affectations",
                table: "Affectations");

            migrationBuilder.RenameTable(
                name: "Affectations",
                newName: "Affectation");

            migrationBuilder.RenameIndex(
                name: "IX_Affectations_OfficialId",
                table: "Affectation",
                newName: "IX_Affectation_OfficialId");

            migrationBuilder.RenameIndex(
                name: "IX_Affectations_EnginId",
                table: "Affectation",
                newName: "IX_Affectation_EnginId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Affectation",
                table: "Affectation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Affectation_Engins_EnginId",
                table: "Affectation",
                column: "EnginId",
                principalTable: "Engins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Affectation_Officials_OfficialId",
                table: "Affectation",
                column: "OfficialId",
                principalTable: "Officials",
                principalColumn: "Id");
        }
    }
}
