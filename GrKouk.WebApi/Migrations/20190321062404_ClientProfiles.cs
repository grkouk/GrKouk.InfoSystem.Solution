using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class ClientProfiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CashRegCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 20, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashRegCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 20, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Serial = table.Column<string>(maxLength: 50, nullable: true),
                    Data = table.Column<string>(maxLength: 200, nullable: true),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientProfiles_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrCatWarehouseItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientProfileId = table.Column<int>(nullable: false),
                    CashRegCategoryId = table.Column<int>(nullable: false),
                    WarehouseItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrCatWarehouseItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrCatWarehouseItems_CashRegCategories_CashRegCategoryId",
                        column: x => x.CashRegCategoryId,
                        principalTable: "CashRegCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrCatWarehouseItems_ClientProfiles_ClientProfileId",
                        column: x => x.ClientProfileId,
                        principalTable: "ClientProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrCatWarehouseItems_WarehouseItems_WarehouseItemId",
                        column: x => x.WarehouseItemId,
                        principalTable: "WarehouseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientProfiles_Code",
                table: "ClientProfiles",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_ClientProfiles_CompanyId",
                table: "ClientProfiles",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CrCatWarehouseItems_CashRegCategoryId",
                table: "CrCatWarehouseItems",
                column: "CashRegCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CrCatWarehouseItems_ClientProfileId",
                table: "CrCatWarehouseItems",
                column: "ClientProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_CrCatWarehouseItems_WarehouseItemId",
                table: "CrCatWarehouseItems",
                column: "WarehouseItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CrCatWarehouseItems");

            migrationBuilder.DropTable(
                name: "CashRegCategories");

            migrationBuilder.DropTable(
                name: "ClientProfiles");
        }
    }
}
