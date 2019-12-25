using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class MaterialCodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WarehouseItemsCodes");

            migrationBuilder.CreateTable(
                name: "WrItemCodes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<int>(nullable: false),
                    CodeType = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 30, nullable: true),
                    TransactorId = table.Column<int>(nullable: false),
                    WarehouseItemId = table.Column<int>(nullable: false),
                    CodeUsedUnit = table.Column<int>(nullable: false),
                    RateToMainUnit = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WrItemCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WrItemCodes_WarehouseItems_WarehouseItemId",
                        column: x => x.WarehouseItemId,
                        principalTable: "WarehouseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WrItemCodes_Code",
                table: "WrItemCodes",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_WrItemCodes_WarehouseItemId",
                table: "WrItemCodes",
                column: "WarehouseItemId");

            migrationBuilder.CreateIndex(
                name: "IX_WrItemCodes_CompanyId_CodeType_WarehouseItemId_TransactorId_Code",
                table: "WrItemCodes",
                columns: new[] { "CompanyId", "CodeType", "WarehouseItemId", "TransactorId", "Code" },
                unique: true,
                filter: "[Code] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WrItemCodes");

            migrationBuilder.CreateTable(
                name: "WarehouseItemsCodes",
                columns: table => new
                {
                    CodeType = table.Column<int>(nullable: false),
                    WarehouseItemId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 30, nullable: false),
                    CodeUsedUnit = table.Column<int>(nullable: false),
                    TransactorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseItemsCodes", x => new { x.CodeType, x.WarehouseItemId, x.Code });
                    table.ForeignKey(
                        name: "FK_WarehouseItemsCodes_WarehouseItems_WarehouseItemId",
                        column: x => x.WarehouseItemId,
                        principalTable: "WarehouseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseItemsCodes_Code",
                table: "WarehouseItemsCodes",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseItemsCodes_WarehouseItemId",
                table: "WarehouseItemsCodes",
                column: "WarehouseItemId");
        }
    }
}
