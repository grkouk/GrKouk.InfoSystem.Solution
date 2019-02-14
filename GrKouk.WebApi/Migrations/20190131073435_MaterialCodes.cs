using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class MaterialCodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaterialCodes",
                columns: table => new
                {
                    CodeType = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 30, nullable: false),
                    TransactorId = table.Column<int>(nullable: false),
                    MaterialId = table.Column<int>(nullable: false),
                    CodeUsedUnit = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialCodes", x => new { x.CodeType, x.MaterialId, x.Code });
                    table.ForeignKey(
                        name: "FK_MaterialCodes_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialCodes_Code",
                table: "MaterialCodes",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialCodes_MaterialId",
                table: "MaterialCodes",
                column: "MaterialId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaterialCodes");
        }
    }
}
