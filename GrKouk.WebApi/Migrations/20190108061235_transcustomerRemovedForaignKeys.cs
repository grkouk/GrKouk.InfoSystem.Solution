using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class transcustomerRemovedForaignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransCustomerDefs_FinancialMovements_CreditTransId",
                table: "TransCustomerDefs");

            migrationBuilder.DropForeignKey(
                name: "FK_TransCustomerDefs_FinancialMovements_DebitTransId",
                table: "TransCustomerDefs");

            migrationBuilder.DropForeignKey(
                name: "FK_TransCustomerDefs_TransCustomerDocSeriesDefs_TransCustomerDefaultDocSeriesId",
                table: "TransCustomerDefs");

            migrationBuilder.DropIndex(
                name: "IX_TransCustomerDefs_CreditTransId",
                table: "TransCustomerDefs");

            migrationBuilder.DropIndex(
                name: "IX_TransCustomerDefs_DebitTransId",
                table: "TransCustomerDefs");

            migrationBuilder.DropIndex(
                name: "IX_TransCustomerDefs_TransCustomerDefaultDocSeriesId",
                table: "TransCustomerDefs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TransCustomerDefs_CreditTransId",
                table: "TransCustomerDefs",
                column: "CreditTransId");

            migrationBuilder.CreateIndex(
                name: "IX_TransCustomerDefs_DebitTransId",
                table: "TransCustomerDefs",
                column: "DebitTransId");

            migrationBuilder.CreateIndex(
                name: "IX_TransCustomerDefs_TransCustomerDefaultDocSeriesId",
                table: "TransCustomerDefs",
                column: "TransCustomerDefaultDocSeriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransCustomerDefs_FinancialMovements_CreditTransId",
                table: "TransCustomerDefs",
                column: "CreditTransId",
                principalTable: "FinancialMovements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransCustomerDefs_FinancialMovements_DebitTransId",
                table: "TransCustomerDefs",
                column: "DebitTransId",
                principalTable: "FinancialMovements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransCustomerDefs_TransCustomerDocSeriesDefs_TransCustomerDefaultDocSeriesId",
                table: "TransCustomerDefs",
                column: "TransCustomerDefaultDocSeriesId",
                principalTable: "TransCustomerDocSeriesDefs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
