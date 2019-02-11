using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class TransWarehouseDef2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmtBuyAction",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "AmtExportsAction",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "AmtImportsAction",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "AmtInvoicedExportsAction",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "AmtInvoicedImportsAction",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "AmtSellAction",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "VolBuyAction",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "VolExportsAction",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "VolImportsAction",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "VolInvoicedExportsAction",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "VolInvoicedImportsAction",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "VolSellAction",
                table: "TransWarehouseDefs");

            migrationBuilder.AddColumn<int>(
                name: "InvoicedValueAction",
                table: "WarehouseTransactions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InvoicedVolumeAction",
                table: "WarehouseTransactions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaterialInvoicedValueAction",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaterialInvoicedVolumeAction",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoicedValueAction",
                table: "WarehouseTransactions");

            migrationBuilder.DropColumn(
                name: "InvoicedVolumeAction",
                table: "WarehouseTransactions");

            migrationBuilder.DropColumn(
                name: "MaterialInvoicedValueAction",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "MaterialInvoicedVolumeAction",
                table: "TransWarehouseDefs");

            migrationBuilder.AddColumn<int>(
                name: "AmtBuyAction",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AmtExportsAction",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AmtImportsAction",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AmtInvoicedExportsAction",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AmtInvoicedImportsAction",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AmtSellAction",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VolBuyAction",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VolExportsAction",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VolImportsAction",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VolInvoicedExportsAction",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VolInvoicedImportsAction",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VolSellAction",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);
        }
    }
}
