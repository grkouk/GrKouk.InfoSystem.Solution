using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class Payoff1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AutoPayoffWay",
                table: "BuyDocSeriesDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PayoffSeriesId",
                table: "BuyDocSeriesDefs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutoPayoffWay",
                table: "BuyDocSeriesDefs");

            migrationBuilder.DropColumn(
                name: "PayoffSeriesId",
                table: "BuyDocSeriesDefs");
        }
    }
}
