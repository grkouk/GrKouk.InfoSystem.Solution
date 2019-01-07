using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class WarehouseTransTypeDefChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransWarehouseDefs_FinancialMovements_AmtExportsTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropForeignKey(
                name: "FK_TransWarehouseDefs_FinancialMovements_AmtImportsTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropForeignKey(
                name: "FK_TransWarehouseDefs_FinancialMovements_VolExportsTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropForeignKey(
                name: "FK_TransWarehouseDefs_FinancialMovements_VolImportsTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropIndex(
                name: "IX_TransWarehouseDefs_AmtExportsTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropIndex(
                name: "IX_TransWarehouseDefs_AmtImportsTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropIndex(
                name: "IX_TransWarehouseDefs_VolExportsTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropIndex(
                name: "IX_TransWarehouseDefs_VolImportsTransId",
                table: "TransWarehouseDefs");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InventoryTransType",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "InventoryValueTransType",
                table: "TransWarehouseDefs");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDefs_AmtExportsTransId",
                table: "TransWarehouseDefs",
                column: "AmtExportsTransId");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDefs_AmtImportsTransId",
                table: "TransWarehouseDefs",
                column: "AmtImportsTransId");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDefs_VolExportsTransId",
                table: "TransWarehouseDefs",
                column: "VolExportsTransId");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDefs_VolImportsTransId",
                table: "TransWarehouseDefs",
                column: "VolImportsTransId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransWarehouseDefs_FinancialMovements_AmtExportsTransId",
                table: "TransWarehouseDefs",
                column: "AmtExportsTransId",
                principalTable: "FinancialMovements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransWarehouseDefs_FinancialMovements_AmtImportsTransId",
                table: "TransWarehouseDefs",
                column: "AmtImportsTransId",
                principalTable: "FinancialMovements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransWarehouseDefs_FinancialMovements_VolExportsTransId",
                table: "TransWarehouseDefs",
                column: "VolExportsTransId",
                principalTable: "FinancialMovements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransWarehouseDefs_FinancialMovements_VolImportsTransId",
                table: "TransWarehouseDefs",
                column: "VolImportsTransId",
                principalTable: "FinancialMovements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
