using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class DiaryDefSelectedTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Definition",
                table: "DiaryDefs");

            migrationBuilder.AddColumn<string>(
                name: "SelectedDocTypes",
                table: "DiaryDefs",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedDocTypes",
                table: "DiaryDefs");

            migrationBuilder.AddColumn<string>(
                name: "Definition",
                table: "DiaryDefs",
                nullable: true);
        }
    }
}
