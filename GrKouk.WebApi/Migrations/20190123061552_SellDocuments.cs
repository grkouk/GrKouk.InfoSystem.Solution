using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class SellDocuments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SellDocTypeDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: false),
                    Abbr = table.Column<string>(maxLength: 20, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    TransTransactorDefId = table.Column<int>(nullable: true),
                    TransWarehouseDefId = table.Column<int>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellDocTypeDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellDocTypeDefs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellDocTypeDefs_TransTransactorDefs_TransTransactorDefId",
                        column: x => x.TransTransactorDefId,
                        principalTable: "TransTransactorDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellDocTypeDefs_TransWarehouseDefs_TransWarehouseDefId",
                        column: x => x.TransWarehouseDefId,
                        principalTable: "TransWarehouseDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SellDocSeriesDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: false),
                    Abbr = table.Column<string>(maxLength: 20, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    SellDocTypeDefId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellDocSeriesDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellDocSeriesDefs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellDocSeriesDefs_SellDocTypeDefs_SellDocTypeDefId",
                        column: x => x.SellDocTypeDefId,
                        principalTable: "SellDocTypeDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SellDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransDate = table.Column<DateTime>(nullable: false),
                    TransRefCode = table.Column<string>(nullable: true),
                    SectionId = table.Column<int>(nullable: false),
                    TransactorId = table.Column<int>(nullable: false),
                    FiscalPeriodId = table.Column<int>(nullable: false),
                    SellDocSeriesId = table.Column<int>(nullable: false),
                    SellDocTypeId = table.Column<int>(nullable: false),
                    AmountFpa = table.Column<decimal>(nullable: false),
                    AmountNet = table.Column<decimal>(nullable: false),
                    AmountDiscount = table.Column<decimal>(nullable: false),
                    Etiology = table.Column<string>(maxLength: 500, nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellDocuments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellDocuments_FiscalPeriods_FiscalPeriodId",
                        column: x => x.FiscalPeriodId,
                        principalTable: "FiscalPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellDocuments_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellDocuments_SellDocSeriesDefs_SellDocSeriesId",
                        column: x => x.SellDocSeriesId,
                        principalTable: "SellDocSeriesDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SellDocuments_SellDocTypeDefs_SellDocTypeId",
                        column: x => x.SellDocTypeId,
                        principalTable: "SellDocTypeDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SellDocuments_Transactors_TransactorId",
                        column: x => x.TransactorId,
                        principalTable: "Transactors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SellDocLines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SellDocumentId = table.Column<int>(nullable: false),
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
                    Etiology = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellDocLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellDocLines_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellDocLines_SellDocuments_SellDocumentId",
                        column: x => x.SellDocumentId,
                        principalTable: "SellDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SellDocLines_MaterialId",
                table: "SellDocLines",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_SellDocLines_SellDocumentId",
                table: "SellDocLines",
                column: "SellDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_SellDocSeriesDefs_Code",
                table: "SellDocSeriesDefs",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SellDocSeriesDefs_CompanyId",
                table: "SellDocSeriesDefs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SellDocSeriesDefs_SellDocTypeDefId",
                table: "SellDocSeriesDefs",
                column: "SellDocTypeDefId");

            migrationBuilder.CreateIndex(
                name: "IX_SellDocTypeDefs_Code",
                table: "SellDocTypeDefs",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SellDocTypeDefs_CompanyId",
                table: "SellDocTypeDefs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SellDocTypeDefs_TransTransactorDefId",
                table: "SellDocTypeDefs",
                column: "TransTransactorDefId");

            migrationBuilder.CreateIndex(
                name: "IX_SellDocTypeDefs_TransWarehouseDefId",
                table: "SellDocTypeDefs",
                column: "TransWarehouseDefId");

            migrationBuilder.CreateIndex(
                name: "IX_SellDocuments_CompanyId",
                table: "SellDocuments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SellDocuments_FiscalPeriodId",
                table: "SellDocuments",
                column: "FiscalPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_SellDocuments_SectionId",
                table: "SellDocuments",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_SellDocuments_SellDocSeriesId",
                table: "SellDocuments",
                column: "SellDocSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_SellDocuments_SellDocTypeId",
                table: "SellDocuments",
                column: "SellDocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SellDocuments_TransDate",
                table: "SellDocuments",
                column: "TransDate");

            migrationBuilder.CreateIndex(
                name: "IX_SellDocuments_TransactorId",
                table: "SellDocuments",
                column: "TransactorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SellDocLines");

            migrationBuilder.DropTable(
                name: "SellDocuments");

            migrationBuilder.DropTable(
                name: "SellDocSeriesDefs");

            migrationBuilder.DropTable(
                name: "SellDocTypeDefs");
        }
    }
}
