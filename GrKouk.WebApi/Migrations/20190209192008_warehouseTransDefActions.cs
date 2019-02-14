using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class warehouseTransDefActions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransWarehouseDefs_FinancialMovements_AmtBuyTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropForeignKey(
                name: "FK_TransWarehouseDefs_FinancialMovements_AmtInvoicedExportsTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropForeignKey(
                name: "FK_TransWarehouseDefs_FinancialMovements_AmtInvoicedImportsTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropForeignKey(
                name: "FK_TransWarehouseDefs_FinancialMovements_AmtSellTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropForeignKey(
                name: "FK_TransWarehouseDefs_FinancialMovements_VolBuyTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropForeignKey(
                name: "FK_TransWarehouseDefs_FinancialMovements_VolInvoicedExportsTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropForeignKey(
                name: "FK_TransWarehouseDefs_FinancialMovements_VolInvoicedImportsTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropForeignKey(
                name: "FK_TransWarehouseDefs_FinancialMovements_VolSellTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropIndex(
                name: "IX_TransWarehouseDefs_AmtBuyTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropIndex(
                name: "IX_TransWarehouseDefs_AmtInvoicedExportsTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropIndex(
                name: "IX_TransWarehouseDefs_AmtInvoicedImportsTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropIndex(
                name: "IX_TransWarehouseDefs_AmtSellTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropIndex(
                name: "IX_TransWarehouseDefs_VolBuyTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropIndex(
                name: "IX_TransWarehouseDefs_VolInvoicedExportsTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropIndex(
                name: "IX_TransWarehouseDefs_VolInvoicedImportsTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropIndex(
                name: "IX_TransWarehouseDefs_VolSellTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "AmtBuyTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "AmtExportsTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "AmtImportsTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "AmtInvoicedExportsTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "AmtInvoicedImportsTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "AmtSellTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "VolBuyTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "VolExportsTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "VolImportsTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "VolInvoicedExportsTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "VolInvoicedImportsTransId",
                table: "TransWarehouseDefs");

            migrationBuilder.DropColumn(
                name: "VolSellTransId",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "AmtBuyTransId",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AmtExportsTransId",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AmtImportsTransId",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AmtInvoicedExportsTransId",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AmtInvoicedImportsTransId",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AmtSellTransId",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VolBuyTransId",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VolExportsTransId",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VolImportsTransId",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VolInvoicedExportsTransId",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VolInvoicedImportsTransId",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VolSellTransId",
                table: "TransWarehouseDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDefs_AmtBuyTransId",
                table: "TransWarehouseDefs",
                column: "AmtBuyTransId");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDefs_AmtInvoicedExportsTransId",
                table: "TransWarehouseDefs",
                column: "AmtInvoicedExportsTransId");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDefs_AmtInvoicedImportsTransId",
                table: "TransWarehouseDefs",
                column: "AmtInvoicedImportsTransId");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDefs_AmtSellTransId",
                table: "TransWarehouseDefs",
                column: "AmtSellTransId");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDefs_VolBuyTransId",
                table: "TransWarehouseDefs",
                column: "VolBuyTransId");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDefs_VolInvoicedExportsTransId",
                table: "TransWarehouseDefs",
                column: "VolInvoicedExportsTransId");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDefs_VolInvoicedImportsTransId",
                table: "TransWarehouseDefs",
                column: "VolInvoicedImportsTransId");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDefs_VolSellTransId",
                table: "TransWarehouseDefs",
                column: "VolSellTransId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransWarehouseDefs_FinancialMovements_AmtBuyTransId",
                table: "TransWarehouseDefs",
                column: "AmtBuyTransId",
                principalTable: "FinancialMovements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransWarehouseDefs_FinancialMovements_AmtInvoicedExportsTransId",
                table: "TransWarehouseDefs",
                column: "AmtInvoicedExportsTransId",
                principalTable: "FinancialMovements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransWarehouseDefs_FinancialMovements_AmtInvoicedImportsTransId",
                table: "TransWarehouseDefs",
                column: "AmtInvoicedImportsTransId",
                principalTable: "FinancialMovements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransWarehouseDefs_FinancialMovements_AmtSellTransId",
                table: "TransWarehouseDefs",
                column: "AmtSellTransId",
                principalTable: "FinancialMovements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransWarehouseDefs_FinancialMovements_VolBuyTransId",
                table: "TransWarehouseDefs",
                column: "VolBuyTransId",
                principalTable: "FinancialMovements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransWarehouseDefs_FinancialMovements_VolInvoicedExportsTransId",
                table: "TransWarehouseDefs",
                column: "VolInvoicedExportsTransId",
                principalTable: "FinancialMovements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransWarehouseDefs_FinancialMovements_VolInvoicedImportsTransId",
                table: "TransWarehouseDefs",
                column: "VolInvoicedImportsTransId",
                principalTable: "FinancialMovements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransWarehouseDefs_FinancialMovements_VolSellTransId",
                table: "TransWarehouseDefs",
                column: "VolSellTransId",
                principalTable: "FinancialMovements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
