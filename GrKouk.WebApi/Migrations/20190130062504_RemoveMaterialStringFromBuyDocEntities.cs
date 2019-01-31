using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class RemoveMaterialStringFromBuyDocEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuyMaterialsDocuments_BuyMaterialDocSeriesDefs_MaterialDocSeriesId",
                table: "BuyMaterialsDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_BuyMaterialsDocuments_BuyMaterialDocTypeDefs_MaterialDocTypeId",
                table: "BuyMaterialsDocuments");

            migrationBuilder.DropTable(
                name: "BuyMaterialDocSeriesDefs");

            migrationBuilder.DropTable(
                name: "BuyMaterialDocTypeDefs");

            migrationBuilder.RenameColumn(
                name: "MaterialDocTypeId",
                table: "BuyMaterialsDocuments",
                newName: "BuyDocTypeId");

            migrationBuilder.RenameColumn(
                name: "MaterialDocSeriesId",
                table: "BuyMaterialsDocuments",
                newName: "BuyDocSeriesId");

            migrationBuilder.RenameIndex(
                name: "IX_BuyMaterialsDocuments_MaterialDocTypeId",
                table: "BuyMaterialsDocuments",
                newName: "IX_BuyMaterialsDocuments_BuyDocTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_BuyMaterialsDocuments_MaterialDocSeriesId",
                table: "BuyMaterialsDocuments",
                newName: "IX_BuyMaterialsDocuments_BuyDocSeriesId");

            migrationBuilder.CreateTable(
                name: "BuyDocTypeDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: false),
                    Abbr = table.Column<string>(maxLength: 20, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    TransSupplierDefId = table.Column<int>(nullable: true),
                    TransTransactorDefId = table.Column<int>(nullable: true),
                    TransWarehouseDefId = table.Column<int>(nullable: true),
                    UsedPrice = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyDocTypeDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuyDocTypeDefs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuyDocTypeDefs_TransSupplierDefs_TransSupplierDefId",
                        column: x => x.TransSupplierDefId,
                        principalTable: "TransSupplierDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuyDocTypeDefs_TransTransactorDefs_TransTransactorDefId",
                        column: x => x.TransTransactorDefId,
                        principalTable: "TransTransactorDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuyDocTypeDefs_TransWarehouseDefs_TransWarehouseDefId",
                        column: x => x.TransWarehouseDefId,
                        principalTable: "TransWarehouseDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BuyDocSeriesDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: false),
                    Abbr = table.Column<string>(maxLength: 20, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    BuyDocTypeDefId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyDocSeriesDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuyDocSeriesDefs_BuyDocTypeDefs_BuyDocTypeDefId",
                        column: x => x.BuyDocTypeDefId,
                        principalTable: "BuyDocTypeDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuyDocSeriesDefs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocSeriesDefs_BuyDocTypeDefId",
                table: "BuyDocSeriesDefs",
                column: "BuyDocTypeDefId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocSeriesDefs_Code",
                table: "BuyDocSeriesDefs",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocSeriesDefs_CompanyId",
                table: "BuyDocSeriesDefs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocTypeDefs_Code",
                table: "BuyDocTypeDefs",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocTypeDefs_CompanyId",
                table: "BuyDocTypeDefs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocTypeDefs_TransSupplierDefId",
                table: "BuyDocTypeDefs",
                column: "TransSupplierDefId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocTypeDefs_TransTransactorDefId",
                table: "BuyDocTypeDefs",
                column: "TransTransactorDefId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocTypeDefs_TransWarehouseDefId",
                table: "BuyDocTypeDefs",
                column: "TransWarehouseDefId");

            migrationBuilder.AddForeignKey(
                name: "FK_BuyMaterialsDocuments_BuyDocSeriesDefs_BuyDocSeriesId",
                table: "BuyMaterialsDocuments",
                column: "BuyDocSeriesId",
                principalTable: "BuyDocSeriesDefs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BuyMaterialsDocuments_BuyDocTypeDefs_BuyDocTypeId",
                table: "BuyMaterialsDocuments",
                column: "BuyDocTypeId",
                principalTable: "BuyDocTypeDefs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuyMaterialsDocuments_BuyDocSeriesDefs_BuyDocSeriesId",
                table: "BuyMaterialsDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_BuyMaterialsDocuments_BuyDocTypeDefs_BuyDocTypeId",
                table: "BuyMaterialsDocuments");

            migrationBuilder.DropTable(
                name: "BuyDocSeriesDefs");

            migrationBuilder.DropTable(
                name: "BuyDocTypeDefs");

            migrationBuilder.RenameColumn(
                name: "BuyDocTypeId",
                table: "BuyMaterialsDocuments",
                newName: "MaterialDocTypeId");

            migrationBuilder.RenameColumn(
                name: "BuyDocSeriesId",
                table: "BuyMaterialsDocuments",
                newName: "MaterialDocSeriesId");

            migrationBuilder.RenameIndex(
                name: "IX_BuyMaterialsDocuments_BuyDocTypeId",
                table: "BuyMaterialsDocuments",
                newName: "IX_BuyMaterialsDocuments_MaterialDocTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_BuyMaterialsDocuments_BuyDocSeriesId",
                table: "BuyMaterialsDocuments",
                newName: "IX_BuyMaterialsDocuments_MaterialDocSeriesId");

            migrationBuilder.CreateTable(
                name: "BuyMaterialDocTypeDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Abbr = table.Column<string>(maxLength: 20, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(maxLength: 15, nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    TransSupplierDefId = table.Column<int>(nullable: true),
                    TransTransactorDefId = table.Column<int>(nullable: true),
                    TransWarehouseDefId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyMaterialDocTypeDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuyMaterialDocTypeDefs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuyMaterialDocTypeDefs_TransSupplierDefs_TransSupplierDefId",
                        column: x => x.TransSupplierDefId,
                        principalTable: "TransSupplierDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuyMaterialDocTypeDefs_TransTransactorDefs_TransTransactorDefId",
                        column: x => x.TransTransactorDefId,
                        principalTable: "TransTransactorDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuyMaterialDocTypeDefs_TransWarehouseDefs_TransWarehouseDefId",
                        column: x => x.TransWarehouseDefId,
                        principalTable: "TransWarehouseDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BuyMaterialDocSeriesDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Abbr = table.Column<string>(maxLength: 20, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    BuyMaterialDocTypeDefId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 15, nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyMaterialDocSeriesDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuyMaterialDocSeriesDefs_BuyMaterialDocTypeDefs_BuyMaterialDocTypeDefId",
                        column: x => x.BuyMaterialDocTypeDefId,
                        principalTable: "BuyMaterialDocTypeDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuyMaterialDocSeriesDefs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BuyMaterialDocSeriesDefs_BuyMaterialDocTypeDefId",
                table: "BuyMaterialDocSeriesDefs",
                column: "BuyMaterialDocTypeDefId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyMaterialDocSeriesDefs_Code",
                table: "BuyMaterialDocSeriesDefs",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BuyMaterialDocSeriesDefs_CompanyId",
                table: "BuyMaterialDocSeriesDefs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyMaterialDocTypeDefs_Code",
                table: "BuyMaterialDocTypeDefs",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BuyMaterialDocTypeDefs_CompanyId",
                table: "BuyMaterialDocTypeDefs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyMaterialDocTypeDefs_TransSupplierDefId",
                table: "BuyMaterialDocTypeDefs",
                column: "TransSupplierDefId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyMaterialDocTypeDefs_TransTransactorDefId",
                table: "BuyMaterialDocTypeDefs",
                column: "TransTransactorDefId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyMaterialDocTypeDefs_TransWarehouseDefId",
                table: "BuyMaterialDocTypeDefs",
                column: "TransWarehouseDefId");

            migrationBuilder.AddForeignKey(
                name: "FK_BuyMaterialsDocuments_BuyMaterialDocSeriesDefs_MaterialDocSeriesId",
                table: "BuyMaterialsDocuments",
                column: "MaterialDocSeriesId",
                principalTable: "BuyMaterialDocSeriesDefs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BuyMaterialsDocuments_BuyMaterialDocTypeDefs_MaterialDocTypeId",
                table: "BuyMaterialsDocuments",
                column: "MaterialDocTypeId",
                principalTable: "BuyMaterialDocTypeDefs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
