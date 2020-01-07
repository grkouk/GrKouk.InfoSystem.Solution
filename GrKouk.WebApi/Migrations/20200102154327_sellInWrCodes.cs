using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class sellInWrCodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BuyCodeUsedUnit",
                table: "WrItemCodes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "BuyRateToMainUnit",
                table: "WrItemCodes",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "SellCodeUsedUnit",
                table: "WrItemCodes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "SellRateToMainUnit",
                table: "WrItemCodes",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyCodeUsedUnit",
                table: "WrItemCodes");

            migrationBuilder.DropColumn(
                name: "BuyRateToMainUnit",
                table: "WrItemCodes");

            migrationBuilder.DropColumn(
                name: "SellCodeUsedUnit",
                table: "WrItemCodes");

            migrationBuilder.DropColumn(
                name: "SellRateToMainUnit",
                table: "WrItemCodes");
        }
    }
}
