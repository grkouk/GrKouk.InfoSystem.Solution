using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class warehouseCodeWarehouse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WarehouseItemId1",
                table: "WarehouseItemsCodes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseItemsCodes_WarehouseItemId1",
                table: "WarehouseItemsCodes",
                column: "WarehouseItemId1");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseItemsCodes_WarehouseItems_WarehouseItemId1",
                table: "WarehouseItemsCodes",
                column: "WarehouseItemId1",
                principalTable: "WarehouseItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseItemsCodes_WarehouseItems_WarehouseItemId1",
                table: "WarehouseItemsCodes");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseItemsCodes_WarehouseItemId1",
                table: "WarehouseItemsCodes");

            migrationBuilder.DropColumn(
                name: "WarehouseItemId1",
                table: "WarehouseItemsCodes");
        }
    }
}
