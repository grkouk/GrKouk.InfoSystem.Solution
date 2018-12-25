using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class noFpaInTransactionEntities2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FpaId",
                table: "BuyMaterialsDocLines");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FpaId",
                table: "BuyMaterialsDocLines",
                nullable: false,
                defaultValue: 0);
        }
    }
}
