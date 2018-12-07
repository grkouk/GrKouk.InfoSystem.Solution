using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class buyMaterialsDocuments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    TransWarehouseDefId = table.Column<int>(nullable: true),
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

            migrationBuilder.CreateTable(
                name: "buyMaterialsDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransDate = table.Column<DateTime>(nullable: false),
                    TransRefCode = table.Column<string>(nullable: true),
                    SectionId = table.Column<int>(nullable: false),
                    SupplierId = table.Column<int>(nullable: false),
                    FiscalPeriodId = table.Column<int>(nullable: false),
                    DocSeriesId = table.Column<int>(nullable: false),
                    DocTypeId = table.Column<int>(nullable: false),
                    AmountFpa = table.Column<decimal>(nullable: false),
                    AmountNet = table.Column<decimal>(nullable: false),
                    AmountDiscount = table.Column<decimal>(nullable: false),
                    Etiology = table.Column<string>(maxLength: 500, nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_buyMaterialsDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_buyMaterialsDocuments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_buyMaterialsDocuments_BuyDocSeriesDefs_DocSeriesId",
                        column: x => x.DocSeriesId,
                        principalTable: "BuyDocSeriesDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_buyMaterialsDocuments_BuyDocTypeDefs_DocTypeId",
                        column: x => x.DocTypeId,
                        principalTable: "BuyDocTypeDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_buyMaterialsDocuments_FiscalPeriod_FiscalPeriodId",
                        column: x => x.FiscalPeriodId,
                        principalTable: "FiscalPeriod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_buyMaterialsDocuments_Section_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Section",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_buyMaterialsDocuments_Transactors_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Transactors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BuyMaterialsDocLines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BuyDocumentId = table.Column<int>(nullable: false),
                    MaterialId = table.Column<int>(nullable: false),
                    FpaId = table.Column<int>(nullable: false),
                    FpaRate = table.Column<float>(nullable: false),
                    PrimaryUnitId = table.Column<int>(nullable: false),
                    SecondaryUnitId = table.Column<int>(nullable: false),
                    Quontity1 = table.Column<double>(nullable: false),
                    Quontity2 = table.Column<double>(nullable: false),
                    AmountFpa = table.Column<decimal>(nullable: false),
                    AmountNet = table.Column<decimal>(nullable: false),
                    DiscountRate = table.Column<float>(nullable: false),
                    Etiology = table.Column<string>(maxLength: 500, nullable: true),
                    BuyMaterialsDocumentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyMaterialsDocLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuyMaterialsDocLines_buyMaterialsDocuments_BuyDocumentId",
                        column: x => x.BuyDocumentId,
                        principalTable: "buyMaterialsDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuyMaterialsDocLines_buyMaterialsDocuments_BuyMaterialsDocumentId",
                        column: x => x.BuyMaterialsDocumentId,
                        principalTable: "buyMaterialsDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuyMaterialsDocLines_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
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
                name: "IX_BuyDocTypeDefs_TransWarehouseDefId",
                table: "BuyDocTypeDefs",
                column: "TransWarehouseDefId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyMaterialsDocLines_BuyDocumentId",
                table: "BuyMaterialsDocLines",
                column: "BuyDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyMaterialsDocLines_BuyMaterialsDocumentId",
                table: "BuyMaterialsDocLines",
                column: "BuyMaterialsDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyMaterialsDocLines_MaterialId",
                table: "BuyMaterialsDocLines",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_buyMaterialsDocuments_CompanyId",
                table: "buyMaterialsDocuments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_buyMaterialsDocuments_DocSeriesId",
                table: "buyMaterialsDocuments",
                column: "DocSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_buyMaterialsDocuments_DocTypeId",
                table: "buyMaterialsDocuments",
                column: "DocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_buyMaterialsDocuments_FiscalPeriodId",
                table: "buyMaterialsDocuments",
                column: "FiscalPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_buyMaterialsDocuments_SectionId",
                table: "buyMaterialsDocuments",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_buyMaterialsDocuments_SupplierId",
                table: "buyMaterialsDocuments",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_buyMaterialsDocuments_TransDate",
                table: "buyMaterialsDocuments",
                column: "TransDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuyMaterialsDocLines");

            migrationBuilder.DropTable(
                name: "buyMaterialsDocuments");

            migrationBuilder.DropTable(
                name: "BuyDocSeriesDefs");

            migrationBuilder.DropTable(
                name: "BuyDocTypeDefs");
        }
    }
}
