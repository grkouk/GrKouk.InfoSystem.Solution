using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class UpdateBuyMaterialsEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TransTransactorDefId",
                table: "BuyMaterialDocTypeDefs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BuyMaterialDocTypeDefs_TransTransactorDefId",
                table: "BuyMaterialDocTypeDefs",
                column: "TransTransactorDefId");

            migrationBuilder.AddForeignKey(
                name: "FK_BuyMaterialDocTypeDefs_TransTransactorDefs_TransTransactorDefId",
                table: "BuyMaterialDocTypeDefs",
                column: "TransTransactorDefId",
                principalTable: "TransTransactorDefs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuyMaterialDocTypeDefs_TransTransactorDefs_TransTransactorDefId",
                table: "BuyMaterialDocTypeDefs");

            migrationBuilder.DropIndex(
                name: "IX_BuyMaterialDocTypeDefs_TransTransactorDefId",
                table: "BuyMaterialDocTypeDefs");

            migrationBuilder.DropColumn(
                name: "TransTransactorDefId",
                table: "BuyMaterialDocTypeDefs");
        }
    }
}
