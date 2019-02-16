using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class DocTypesAddedWarehouseNameture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SelectedWarehouseItemNatures",
                table: "SellDocTypeDefs",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SelectedWarehouseItemNatures",
                table: "BuyDocTypeDefs",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedWarehouseItemNatures",
                table: "SellDocTypeDefs");

            migrationBuilder.DropColumn(
                name: "SelectedWarehouseItemNatures",
                table: "BuyDocTypeDefs");
        }
    }
}
