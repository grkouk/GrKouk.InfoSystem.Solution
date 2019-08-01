using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class PaymentMethod3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BuyDocuments_PaymentMethodId",
                table: "BuyDocuments",
                column: "PaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_BuyDocuments_PaymentMethods_PaymentMethodId",
                table: "BuyDocuments",
                column: "PaymentMethodId",
                principalTable: "PaymentMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuyDocuments_PaymentMethods_PaymentMethodId",
                table: "BuyDocuments");

            migrationBuilder.DropIndex(
                name: "IX_BuyDocuments_PaymentMethodId",
                table: "BuyDocuments");
        }
    }
}
