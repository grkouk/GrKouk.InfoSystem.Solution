using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class supplierTransactionChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransSupplierDefs_FinancialMovements_CreditTransId",
                table: "TransSupplierDefs");

            migrationBuilder.DropForeignKey(
                name: "FK_TransSupplierDefs_FinancialMovements_DebitTransId",
                table: "TransSupplierDefs");

            migrationBuilder.DropIndex(
                name: "IX_TransSupplierDefs_CreditTransId",
                table: "TransSupplierDefs");

            migrationBuilder.DropIndex(
                name: "IX_TransSupplierDefs_DebitTransId",
                table: "TransSupplierDefs");

            migrationBuilder.AddColumn<int>(
                name: "FinancialTransType",
                table: "TransSupplierDefs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinancialTransType",
                table: "TransSupplierDefs");

            migrationBuilder.CreateIndex(
                name: "IX_TransSupplierDefs_CreditTransId",
                table: "TransSupplierDefs",
                column: "CreditTransId");

            migrationBuilder.CreateIndex(
                name: "IX_TransSupplierDefs_DebitTransId",
                table: "TransSupplierDefs",
                column: "DebitTransId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransSupplierDefs_FinancialMovements_CreditTransId",
                table: "TransSupplierDefs",
                column: "CreditTransId",
                principalTable: "FinancialMovements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransSupplierDefs_FinancialMovements_DebitTransId",
                table: "TransSupplierDefs",
                column: "DebitTransId",
                principalTable: "FinancialMovements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
