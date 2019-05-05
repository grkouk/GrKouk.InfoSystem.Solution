using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class sellPaymentmethod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodId",
                table: "SellDocuments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AutoPayoffWay",
                table: "SellDocSeriesDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PayoffSeriesId",
                table: "SellDocSeriesDefs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                table: "SellDocuments");

            migrationBuilder.DropColumn(
                name: "AutoPayoffWay",
                table: "SellDocSeriesDefs");

            migrationBuilder.DropColumn(
                name: "PayoffSeriesId",
                table: "SellDocSeriesDefs");
        }
    }
}
