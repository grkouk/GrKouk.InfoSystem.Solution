using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class DefaultSeriesInTransactionDefinitions2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransWarehouseDocSeriesDefId",
                table: "TransWarehouseDefs",
                newName: "TransWarehouseDefaultDocSeriesDefId");

            migrationBuilder.RenameColumn(
                name: "TransSupplierDocSeriesId",
                table: "TransSupplierDefs",
                newName: "TransSupplierDefaultDocSeriesId");

            migrationBuilder.AddColumn<int>(
                name: "TransCustomerDefaultDocSeriesId",
                table: "TransCustomerDefs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransCustomerDefaultDocSeriesId",
                table: "TransCustomerDefs");

            migrationBuilder.RenameColumn(
                name: "TransWarehouseDefaultDocSeriesDefId",
                table: "TransWarehouseDefs",
                newName: "TransWarehouseDocSeriesDefId");

            migrationBuilder.RenameColumn(
                name: "TransSupplierDefaultDocSeriesId",
                table: "TransSupplierDefs",
                newName: "TransSupplierDocSeriesId");
        }
    }
}
