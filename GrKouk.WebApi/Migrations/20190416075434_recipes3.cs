using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class recipes3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GlobalSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductProduceSeriesId = table.Column<int>(nullable: false),
                    RawMaterialConsumeSeriesId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GlobalSettings_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductRecipes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<int>(nullable: false),
                    PrimaryUnitId = table.Column<int>(nullable: false),
                    SecondaryUnitId = table.Column<int>(nullable: false),
                    Factor = table.Column<float>(nullable: false),
                    Quantity1 = table.Column<double>(nullable: false),
                    Quantity2 = table.Column<double>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRecipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductRecipes_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductRecipes_WarehouseItems_ProductId",
                        column: x => x.ProductId,
                        principalTable: "WarehouseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductRecipeLines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductRecipeId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    PrimaryUnitId = table.Column<int>(nullable: false),
                    SecondaryUnitId = table.Column<int>(nullable: false),
                    Factor = table.Column<float>(nullable: false),
                    Quantity1 = table.Column<double>(nullable: false),
                    Quantity2 = table.Column<double>(nullable: false),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    UnitExpenses = table.Column<decimal>(nullable: false),
                    AmountNet = table.Column<decimal>(nullable: false),
                    Etiology = table.Column<string>(maxLength: 500, nullable: true),
                    ProductRecipeId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRecipeLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductRecipeLines_WarehouseItems_ProductId",
                        column: x => x.ProductId,
                        principalTable: "WarehouseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductRecipeLines_ProductRecipes_ProductRecipeId",
                        column: x => x.ProductRecipeId,
                        principalTable: "ProductRecipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductRecipeLines_ProductRecipes_ProductRecipeId1",
                        column: x => x.ProductRecipeId1,
                        principalTable: "ProductRecipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GlobalSettings_CompanyId",
                table: "GlobalSettings",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecipeLines_ProductId",
                table: "ProductRecipeLines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecipeLines_ProductRecipeId",
                table: "ProductRecipeLines",
                column: "ProductRecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecipeLines_ProductRecipeId1",
                table: "ProductRecipeLines",
                column: "ProductRecipeId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecipes_CompanyId",
                table: "ProductRecipes",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecipes_ProductId",
                table: "ProductRecipes",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GlobalSettings");

            migrationBuilder.DropTable(
                name: "ProductRecipeLines");

            migrationBuilder.DropTable(
                name: "ProductRecipes");
        }
    }
}
