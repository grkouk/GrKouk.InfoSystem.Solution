using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class DefaultSeriesInTransactionDefinitions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TransWarehouseDocSeriesDefId",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TransSupplierDocSeriesId",
                table: "TransSupplierDefs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransWarehouseDocSeriesDefId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "TransSupplierDocSeriesId",
                table: "TransSupplierDefs");
        }
    }
}
