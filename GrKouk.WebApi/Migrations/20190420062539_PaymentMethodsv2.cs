using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class PaymentMethodsv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AutoPayoffWay",
                table: "PaymentMethods",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PayoffSeriesId",
                table: "PaymentMethods",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutoPayoffWay",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "PayoffSeriesId",
                table: "PaymentMethods");
        }
    }
}
