using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class TransactionsEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FiscalPeriod",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FiscalPeriod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Section",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    SystemName = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Section", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SupplierTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransDate = table.Column<DateTime>(nullable: false),
                    TransSupplierDocSeriesId = table.Column<int>(nullable: false),
                    TransSupplierDocTypeId = table.Column<int>(nullable: false),
                    TransRefCode = table.Column<string>(nullable: true),
                    SectionId = table.Column<int>(nullable: false),
                    SupplierId = table.Column<int>(nullable: false),
                    FiscalPeriodId = table.Column<int>(nullable: false),
                    TransactionType = table.Column<int>(nullable: false),
                    CreatorId = table.Column<int>(nullable: false),
                    FpaDefId = table.Column<int>(nullable: false),
                    FpaRate = table.Column<float>(nullable: false),
                    AmountFpa = table.Column<decimal>(nullable: false),
                    AmountNet = table.Column<decimal>(nullable: false),
                    AmountDiscount = table.Column<decimal>(nullable: false),
                    Etiology = table.Column<string>(maxLength: 500, nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierTransactions_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupplierTransactions_FiscalPeriod_FiscalPeriodId",
                        column: x => x.FiscalPeriodId,
                        principalTable: "FiscalPeriod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupplierTransactions_FpaKategories_FpaDefId",
                        column: x => x.FpaDefId,
                        principalTable: "FpaKategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupplierTransactions_Section_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Section",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupplierTransactions_Transactors_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Transactors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupplierTransactions_TransSupplierDocSeriesDefs_TransSupplierDocSeriesId",
                        column: x => x.TransSupplierDocSeriesId,
                        principalTable: "TransSupplierDocSeriesDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupplierTransactions_TransSupplierDocTypeDefs_TransSupplierDocTypeId",
                        column: x => x.TransSupplierDocTypeId,
                        principalTable: "TransSupplierDocTypeDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransDate = table.Column<DateTime>(nullable: false),
                    TransWarehouseDocSeriesId = table.Column<int>(nullable: false),
                    TransWarehouseDocTypeId = table.Column<int>(nullable: false),
                    TransRefCode = table.Column<string>(nullable: true),
                    SectionId = table.Column<int>(nullable: false),
                    MaterialId = table.Column<int>(nullable: false),
                    FiscalPeriodId = table.Column<int>(nullable: false),
                    TransactionType = table.Column<int>(nullable: false),
                    CreatorId = table.Column<int>(nullable: false),
                    FpaId = table.Column<int>(nullable: false),
                    FpaDefId = table.Column<int>(nullable: true),
                    FpaRate = table.Column<float>(nullable: false),
                    PrimaryUnitId = table.Column<int>(nullable: false),
                    SecondaryUnitId = table.Column<int>(nullable: false),
                    Quontity1 = table.Column<double>(nullable: false),
                    Quontity2 = table.Column<double>(nullable: false),
                    AmountFpa = table.Column<decimal>(nullable: false),
                    AmountNet = table.Column<decimal>(nullable: false),
                    AmountDiscount = table.Column<decimal>(nullable: false),
                    Etiology = table.Column<string>(maxLength: 500, nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseTransactions_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseTransactions_FiscalPeriod_FiscalPeriodId",
                        column: x => x.FiscalPeriodId,
                        principalTable: "FiscalPeriod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseTransactions_FpaKategories_FpaDefId",
                        column: x => x.FpaDefId,
                        principalTable: "FpaKategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseTransactions_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseTransactions_Section_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Section",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseTransactions_TransWarehouseDocSeriesDefs_TransWarehouseDocSeriesId",
                        column: x => x.TransWarehouseDocSeriesId,
                        principalTable: "TransWarehouseDocSeriesDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseTransactions_TransWarehouseDocTypeDefs_TransWarehouseDocTypeId",
                        column: x => x.TransWarehouseDocTypeId,
                        principalTable: "TransWarehouseDocTypeDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupplierTransactions_CompanyId",
                table: "SupplierTransactions",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierTransactions_CreatorId",
                table: "SupplierTransactions",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierTransactions_FiscalPeriodId",
                table: "SupplierTransactions",
                column: "FiscalPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierTransactions_FpaDefId",
                table: "SupplierTransactions",
                column: "FpaDefId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierTransactions_SectionId",
                table: "SupplierTransactions",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierTransactions_SupplierId",
                table: "SupplierTransactions",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierTransactions_TransDate",
                table: "SupplierTransactions",
                column: "TransDate");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierTransactions_TransSupplierDocSeriesId",
                table: "SupplierTransactions",
                column: "TransSupplierDocSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierTransactions_TransSupplierDocTypeId",
                table: "SupplierTransactions",
                column: "TransSupplierDocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTransactions_CompanyId",
                table: "WarehouseTransactions",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTransactions_CreatorId",
                table: "WarehouseTransactions",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTransactions_FiscalPeriodId",
                table: "WarehouseTransactions",
                column: "FiscalPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTransactions_FpaDefId",
                table: "WarehouseTransactions",
                column: "FpaDefId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTransactions_MaterialId",
                table: "WarehouseTransactions",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTransactions_SectionId",
                table: "WarehouseTransactions",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTransactions_TransDate",
                table: "WarehouseTransactions",
                column: "TransDate");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTransactions_TransWarehouseDocSeriesId",
                table: "WarehouseTransactions",
                column: "TransWarehouseDocSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTransactions_TransWarehouseDocTypeId",
                table: "WarehouseTransactions",
                column: "TransWarehouseDocTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupplierTransactions");

            migrationBuilder.DropTable(
                name: "WarehouseTransactions");

            migrationBuilder.DropTable(
                name: "FiscalPeriod");

            migrationBuilder.DropTable(
                name: "Section");
        }
    }
}
