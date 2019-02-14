using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class FinTransAction2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InventoryTransType",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "InventoryValueTransType",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "FinancialTransAction",
                table: "TransSupplierDefs");

            migrationBuilder.RenameColumn(
                name: "TransWarehouseDefaultDocSeriesDefId",
                table: "TransWarehouseDefs",
                newName: "DefaultDocSeriesId");

            migrationBuilder.AddColumn<int>(
                name: "InventoryAction",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InventoryValueAction",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FinancialAction",
                table: "TransSupplierDefs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InventoryAction",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "InventoryValueAction",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "FinancialAction",
                table: "TransSupplierDefs");

            migrationBuilder.RenameColumn(
                name: "DefaultDocSeriesId",
                table: "TransWarehouseDefs",
                newName: "TransWarehouseDefaultDocSeriesDefId");

            migrationBuilder.AddColumn<int>(
                name: "InventoryTransType",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InventoryValueTransType",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FinancialTransAction",
                table: "TransSupplierDefs",
                nullable: false,
                defaultValue: 0);
        }
    }
}
