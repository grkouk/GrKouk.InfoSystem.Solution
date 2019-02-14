using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class removedefautseries2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransWarehouseDefs_TransWarehouseDocSeriesDefs_TransWarehouseDefaultDocSeriesDefId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropIndex(
                name: "IX_TransWarehouseDefs_TransWarehouseDefaultDocSeriesDefId",
                table: "TransWarehouseDefs");

            migrationBuilder.CreateIndex(
                name: "IX_TransSupplierDefs_TransSupplierDefaultDocSeriesId",
                table: "TransSupplierDefs",
                column: "TransSupplierDefaultDocSeriesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TransSupplierDefs_TransSupplierDefaultDocSeriesId",
                table: "TransSupplierDefs");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDefs_TransWarehouseDefaultDocSeriesDefId",
                table: "TransWarehouseDefs",
                column: "TransWarehouseDefaultDocSeriesDefId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransWarehouseDefs_TransWarehouseDocSeriesDefs_TransWarehouseDefaultDocSeriesDefId",
                table: "TransWarehouseDefs",
                column: "TransWarehouseDefaultDocSeriesDefId",
                principalTable: "TransWarehouseDocSeriesDefs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
