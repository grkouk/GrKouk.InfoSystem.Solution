using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class RatesForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_ClosingDate",
                table: "ExchangeRates",
                column: "ClosingDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExchangeRates_ClosingDate",
                table: "ExchangeRates");
        }
    }
}
