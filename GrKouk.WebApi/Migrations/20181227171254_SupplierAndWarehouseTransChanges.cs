using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class SupplierAndWarehouseTransChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InventoryAction",
                table: "WarehouseTransactions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InventoryValueAction",
                table: "WarehouseTransactions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FinancialAction",
                table: "SupplierTransactions",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InventoryAction",
                table: "WarehouseTransactions");

            migrationBuilder.DropColumn(
                name: "InventoryValueAction",
                table: "WarehouseTransactions");

            migrationBuilder.DropColumn(
                name: "FinancialAction",
                table: "SupplierTransactions");
        }
    }
}
