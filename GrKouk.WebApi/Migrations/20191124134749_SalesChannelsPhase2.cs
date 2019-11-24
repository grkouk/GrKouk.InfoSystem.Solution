using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class SalesChannelsPhase2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SalesChannelId",
                table: "SellDocuments",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SellDocuments_SalesChannelId",
                table: "SellDocuments",
                column: "SalesChannelId");

            migrationBuilder.AddForeignKey(
                name: "FK_SellDocuments_SalesChannels_SalesChannelId",
                table: "SellDocuments",
                column: "SalesChannelId",
                principalTable: "SalesChannels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellDocuments_SalesChannels_SalesChannelId",
                table: "SellDocuments");

            migrationBuilder.DropIndex(
                name: "IX_SellDocuments_SalesChannelId",
                table: "SellDocuments");

            migrationBuilder.AlterColumn<int>(
                name: "SalesChannelId",
                table: "SellDocuments",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
