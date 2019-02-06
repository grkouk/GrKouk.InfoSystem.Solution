using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class Diarys3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SelectedMatNatures",
                table: "DiaryDefs",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SelectedTransTypes",
                table: "DiaryDefs",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedMatNatures",
                table: "DiaryDefs");

            migrationBuilder.DropColumn(
                name: "SelectedTransTypes",
                table: "DiaryDefs");
        }
    }
}
