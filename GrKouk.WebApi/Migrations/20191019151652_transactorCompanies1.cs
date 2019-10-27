using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class transactorCompanies1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactors_Companies_CompanyId",
                table: "Transactors");

            migrationBuilder.CreateTable(
                name: "TransactorCompanyMappings",
                columns: table => new
                {
                    TransactorId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactorCompanyMappings", x => new { x.CompanyId, x.TransactorId });
                    table.ForeignKey(
                        name: "FK_TransactorCompanyMappings_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactorCompanyMappings_Transactors_TransactorId",
                        column: x => x.TransactorId,
                        principalTable: "Transactors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactorCompanyMappings_TransactorId",
                table: "TransactorCompanyMappings",
                column: "TransactorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactors_Companies_CompanyId",
                table: "Transactors",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactors_Companies_CompanyId",
                table: "Transactors");

            migrationBuilder.DropTable(
                name: "TransactorCompanyMappings");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactors_Companies_CompanyId",
                table: "Transactors",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
