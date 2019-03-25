using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class ClientRegisterCatProdIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CrCatWarehouseItems_ClientProfileId",
                table: "CrCatWarehouseItems");

            migrationBuilder.CreateIndex(
                name: "IX_CrCatWarehouseItems_ClientProfileId_CashRegCategoryId_WarehouseItemId",
                table: "CrCatWarehouseItems",
                columns: new[] { "ClientProfileId", "CashRegCategoryId", "WarehouseItemId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CrCatWarehouseItems_ClientProfileId_CashRegCategoryId_WarehouseItemId",
                table: "CrCatWarehouseItems");

            migrationBuilder.CreateIndex(
                name: "IX_CrCatWarehouseItems_ClientProfileId",
                table: "CrCatWarehouseItems",
                column: "ClientProfileId");
        }
    }
}
