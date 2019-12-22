using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class TransactorRemovedCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactors_Companies_CompanyId",
                table: "Transactors");

            migrationBuilder.DropIndex(
                name: "IX_Transactors_CompanyId",
                table: "Transactors");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Transactors");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Transactors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transactors_CompanyId",
                table: "Transactors",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactors_Companies_CompanyId",
                table: "Transactors",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
