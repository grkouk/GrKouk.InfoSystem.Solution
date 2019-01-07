using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class DefaultSeriesInTransactionDefinitions3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TransSupplierDefaultDocSeriesId",
                table: "TransSupplierDefs",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDefs_TransWarehouseDefaultDocSeriesDefId",
                table: "TransWarehouseDefs",
                column: "TransWarehouseDefaultDocSeriesDefId");

            migrationBuilder.CreateIndex(
                name: "IX_TransSupplierDefs_TransSupplierDefaultDocSeriesId",
                table: "TransSupplierDefs",
                column: "TransSupplierDefaultDocSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_TransCustomerDefs_TransCustomerDefaultDocSeriesId",
                table: "TransCustomerDefs",
                column: "TransCustomerDefaultDocSeriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransCustomerDefs_TransCustomerDocSeriesDefs_TransCustomerDefaultDocSeriesId",
                table: "TransCustomerDefs",
                column: "TransCustomerDefaultDocSeriesId",
                principalTable: "TransCustomerDocSeriesDefs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransSupplierDefs_TransSupplierDocSeriesDefs_TransSupplierDefaultDocSeriesId",
                table: "TransSupplierDefs",
                column: "TransSupplierDefaultDocSeriesId",
                principalTable: "TransSupplierDocSeriesDefs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransWarehouseDefs_TransWarehouseDocSeriesDefs_TransWarehouseDefaultDocSeriesDefId",
                table: "TransWarehouseDefs",
                column: "TransWarehouseDefaultDocSeriesDefId",
                principalTable: "TransWarehouseDocSeriesDefs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransCustomerDefs_TransCustomerDocSeriesDefs_TransCustomerDefaultDocSeriesId",
                table: "TransCustomerDefs");

            migrationBuilder.DropForeignKey(
                name: "FK_TransSupplierDefs_TransSupplierDocSeriesDefs_TransSupplierDefaultDocSeriesId",
                table: "TransSupplierDefs");

            migrationBuilder.DropForeignKey(
                name: "FK_TransWarehouseDefs_TransWarehouseDocSeriesDefs_TransWarehouseDefaultDocSeriesDefId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropIndex(
                name: "IX_TransWarehouseDefs_TransWarehouseDefaultDocSeriesDefId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropIndex(
                name: "IX_TransSupplierDefs_TransSupplierDefaultDocSeriesId",
                table: "TransSupplierDefs");

            migrationBuilder.DropIndex(
                name: "IX_TransCustomerDefs_TransCustomerDefaultDocSeriesId",
                table: "TransCustomerDefs");

            migrationBuilder.AlterColumn<int>(
                name: "TransSupplierDefaultDocSeriesId",
                table: "TransSupplierDefs",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
