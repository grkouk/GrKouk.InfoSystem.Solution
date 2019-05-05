using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class sellPaymentmethod1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SellDocuments_PaymentMethodId",
                table: "SellDocuments",
                column: "PaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_SellDocuments_PaymentMethods_PaymentMethodId",
                table: "SellDocuments",
                column: "PaymentMethodId",
                principalTable: "PaymentMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellDocuments_PaymentMethods_PaymentMethodId",
                table: "SellDocuments");

            migrationBuilder.DropIndex(
                name: "IX_SellDocuments_PaymentMethodId",
                table: "SellDocuments");
        }
    }
}
