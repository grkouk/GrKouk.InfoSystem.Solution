using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class RemoveMaterialStringFromBuyDocEntities2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuyMaterialsDocLines");

            migrationBuilder.DropTable(
                name: "BuyMaterialsDocuments");

            migrationBuilder.CreateTable(
                name: "BuyDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransDate = table.Column<DateTime>(nullable: false),
                    TransRefCode = table.Column<string>(nullable: true),
                    SectionId = table.Column<int>(nullable: false),
                    TransactorId = table.Column<int>(nullable: false),
                    FiscalPeriodId = table.Column<int>(nullable: false),
                    BuyDocSeriesId = table.Column<int>(nullable: false),
                    BuyDocTypeId = table.Column<int>(nullable: false),
                    AmountFpa = table.Column<decimal>(nullable: false),
                    AmountNet = table.Column<decimal>(nullable: false),
                    AmountDiscount = table.Column<decimal>(nullable: false),
                    Etiology = table.Column<string>(maxLength: 500, nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuyDocuments_BuyDocSeriesDefs_BuyDocSeriesId",
                        column: x => x.BuyDocSeriesId,
                        principalTable: "BuyDocSeriesDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BuyDocuments_BuyDocTypeDefs_BuyDocTypeId",
                        column: x => x.BuyDocTypeId,
                        principalTable: "BuyDocTypeDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BuyDocuments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuyDocuments_FiscalPeriods_FiscalPeriodId",
                        column: x => x.FiscalPeriodId,
                        principalTable: "FiscalPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuyDocuments_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuyDocuments_Transactors_TransactorId",
                        column: x => x.TransactorId,
                        principalTable: "Transactors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BuyDocLines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BuyDocumentId = table.Column<int>(nullable: false),
                    MaterialId = table.Column<int>(nullable: false),
                    PrimaryUnitId = table.Column<int>(nullable: false),
                    SecondaryUnitId = table.Column<int>(nullable: false),
                    Factor = table.Column<float>(nullable: false),
                    Quontity1 = table.Column<double>(nullable: false),
                    Quontity2 = table.Column<double>(nullable: false),
                    FpaRate = table.Column<decimal>(nullable: false),
                    DiscountRate = table.Column<decimal>(nullable: false),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    AmountFpa = table.Column<decimal>(nullable: false),
                    AmountNet = table.Column<decimal>(nullable: false),
                    AmountDiscount = table.Column<decimal>(nullable: false),
                    Etiology = table.Column<string>(maxLength: 500, nullable: true),
                    BuyDocumentId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyDocLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuyDocLines_BuyDocuments_BuyDocumentId",
                        column: x => x.BuyDocumentId,
                        principalTable: "BuyDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuyDocLines_BuyDocuments_BuyDocumentId1",
                        column: x => x.BuyDocumentId1,
                        principalTable: "BuyDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuyDocLines_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocLines_BuyDocumentId",
                table: "BuyDocLines",
                column: "BuyDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocLines_BuyDocumentId1",
                table: "BuyDocLines",
                column: "BuyDocumentId1");

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocLines_MaterialId",
                table: "BuyDocLines",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocuments_BuyDocSeriesId",
                table: "BuyDocuments",
                column: "BuyDocSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocuments_BuyDocTypeId",
                table: "BuyDocuments",
                column: "BuyDocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocuments_CompanyId",
                table: "BuyDocuments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocuments_FiscalPeriodId",
                table: "BuyDocuments",
                column: "FiscalPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocuments_SectionId",
                table: "BuyDocuments",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocuments_TransDate",
                table: "BuyDocuments",
                column: "TransDate");

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocuments_TransactorId",
                table: "BuyDocuments",
                column: "TransactorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuyDocLines");

            migrationBuilder.DropTable(
                name: "BuyDocuments");

            migrationBuilder.CreateTable(
                name: "BuyMaterialsDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AmountDiscount = table.Column<decimal>(nullable: false),
                    AmountFpa = table.Column<decimal>(nullable: false),
                    AmountNet = table.Column<decimal>(nullable: false),
                    BuyDocSeriesId = table.Column<int>(nullable: false),
                    BuyDocTypeId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    Etiology = table.Column<string>(maxLength: 500, nullable: true),
                    FiscalPeriodId = table.Column<int>(nullable: false),
                    SectionId = table.Column<int>(nullable: false),
                    SupplierId = table.Column<int>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    TransDate = table.Column<DateTime>(nullable: false),
                    TransRefCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyMaterialsDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuyMaterialsDocuments_BuyDocSeriesDefs_BuyDocSeriesId",
                        column: x => x.BuyDocSeriesId,
                        principalTable: "BuyDocSeriesDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BuyMaterialsDocuments_BuyDocTypeDefs_BuyDocTypeId",
                        column: x => x.BuyDocTypeId,
                        principalTable: "BuyDocTypeDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BuyMaterialsDocuments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuyMaterialsDocuments_FiscalPeriods_FiscalPeriodId",
                        column: x => x.FiscalPeriodId,
                        principalTable: "FiscalPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuyMaterialsDocuments_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuyMaterialsDocuments_Transactors_SupplierId",
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
                    AmountDiscount = table.Column<decimal>(nullable: false),
                    AmountFpa = table.Column<decimal>(nullable: false),
                    AmountNet = table.Column<decimal>(nullable: false),
                    BuyDocumentId = table.Column<int>(nullable: false),
                    BuyMaterialsDocumentId = table.Column<int>(nullable: true),
                    DiscountRate = table.Column<decimal>(nullable: false),
                    Etiology = table.Column<string>(maxLength: 500, nullable: true),
                    Factor = table.Column<float>(nullable: false),
                    FpaRate = table.Column<decimal>(nullable: false),
                    MaterialId = table.Column<int>(nullable: false),
                    PrimaryUnitId = table.Column<int>(nullable: false),
                    Quontity1 = table.Column<double>(nullable: false),
                    Quontity2 = table.Column<double>(nullable: false),
                    SecondaryUnitId = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyMaterialsDocLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuyMaterialsDocLines_BuyMaterialsDocuments_BuyDocumentId",
                        column: x => x.BuyDocumentId,
                        principalTable: "BuyMaterialsDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuyMaterialsDocLines_BuyMaterialsDocuments_BuyMaterialsDocumentId",
                        column: x => x.BuyMaterialsDocumentId,
                        principalTable: "BuyMaterialsDocuments",
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
                name: "IX_BuyMaterialsDocuments_BuyDocSeriesId",
                table: "BuyMaterialsDocuments",
                column: "BuyDocSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyMaterialsDocuments_BuyDocTypeId",
                table: "BuyMaterialsDocuments",
                column: "BuyDocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyMaterialsDocuments_CompanyId",
                table: "BuyMaterialsDocuments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyMaterialsDocuments_FiscalPeriodId",
                table: "BuyMaterialsDocuments",
                column: "FiscalPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyMaterialsDocuments_SectionId",
                table: "BuyMaterialsDocuments",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyMaterialsDocuments_SupplierId",
                table: "BuyMaterialsDocuments",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyMaterialsDocuments_TransDate",
                table: "BuyMaterialsDocuments",
                column: "TransDate");
        }
    }
}
