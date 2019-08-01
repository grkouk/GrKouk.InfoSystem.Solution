using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class RawMaterial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RawMaterialInventoryAction",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RawMaterialInventoryValueAction",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitExpenses",
                table: "BuyDocLines",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RawMaterialInventoryAction",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "RawMaterialInventoryValueAction",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "UnitExpenses",
                table: "BuyDocLines");
        }
    }
}
