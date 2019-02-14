using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class WarehouseTransactionTransValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TransDiscountAmount",
                table: "WarehouseTransactions",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TransFpaAmount",
                table: "WarehouseTransactions",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TransNetAmount",
                table: "WarehouseTransactions",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TransQ1",
                table: "WarehouseTransactions",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TransQ2",
                table: "WarehouseTransactions",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransDiscountAmount",
                table: "WarehouseTransactions");

            migrationBuilder.DropColumn(
                name: "TransFpaAmount",
                table: "WarehouseTransactions");

            migrationBuilder.DropColumn(
                name: "TransNetAmount",
                table: "WarehouseTransactions");

            migrationBuilder.DropColumn(
                name: "TransQ1",
                table: "WarehouseTransactions");

            migrationBuilder.DropColumn(
                name: "TransQ2",
                table: "WarehouseTransactions");
        }
    }
}
