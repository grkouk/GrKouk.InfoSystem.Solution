using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class MaterialNullValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SecondaryMeasureUnitId",
                table: "Materials",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MainMeasureUnitId",
                table: "Materials",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BuyMeasureUnitId",
                table: "Materials",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SecondaryMeasureUnitId",
                table: "Materials",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "MainMeasureUnitId",
                table: "Materials",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "BuyMeasureUnitId",
                table: "Materials",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
