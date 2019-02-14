using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class WarehouseTransdefNewDefinition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InventoryAction",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "InventoryValueAction",
                table: "TransWarehouseDefs");

            migrationBuilder.AddColumn<int>(
                name: "ExpenseInventoryAction",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExpenseInventoryValueAction",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FixedAssetInventoryAction",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FixedAssetInventoryValueAction",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IncomeInventoryAction",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IncomeInventoryValueAction",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaterialInventoryAction",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaterialInventoryValueAction",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ServiceInventoryAction",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ServiceInventoryValueAction",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpenseInventoryAction",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "ExpenseInventoryValueAction",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "FixedAssetInventoryAction",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "FixedAssetInventoryValueAction",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "IncomeInventoryAction",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "IncomeInventoryValueAction",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "MaterialInventoryAction",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "MaterialInventoryValueAction",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "ServiceInventoryAction",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "ServiceInventoryValueAction",
                table: "TransWarehouseDefs");

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
        }
    }
}
