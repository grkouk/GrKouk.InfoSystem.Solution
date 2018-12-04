using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class CustomerTransactionEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransDate = table.Column<DateTime>(nullable: false),
                    TransCustomerDocSeriesId = table.Column<int>(nullable: false),
                    TransCustomerDocTypeId = table.Column<int>(nullable: false),
                    TransRefCode = table.Column<string>(nullable: true),
                    SectionId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
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
                    table.PrimaryKey("PK_CustomerTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerTransactions_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerTransactions_Transactors_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Transactors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerTransactions_FiscalPeriod_FiscalPeriodId",
                        column: x => x.FiscalPeriodId,
                        principalTable: "FiscalPeriod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerTransactions_FpaKategories_FpaDefId",
                        column: x => x.FpaDefId,
                        principalTable: "FpaKategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerTransactions_Section_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Section",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerTransactions_TransCustomerDocSeriesDefs_TransCustomerDocSeriesId",
                        column: x => x.TransCustomerDocSeriesId,
                        principalTable: "TransCustomerDocSeriesDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerTransactions_TransCustomerDocTypeDefs_TransCustomerDocTypeId",
                        column: x => x.TransCustomerDocTypeId,
                        principalTable: "TransCustomerDocTypeDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTransactions_CompanyId",
                table: "CustomerTransactions",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTransactions_CreatorId",
                table: "CustomerTransactions",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTransactions_CustomerId",
                table: "CustomerTransactions",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTransactions_FiscalPeriodId",
                table: "CustomerTransactions",
                column: "FiscalPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTransactions_FpaDefId",
                table: "CustomerTransactions",
                column: "FpaDefId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTransactions_SectionId",
                table: "CustomerTransactions",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTransactions_TransCustomerDocSeriesId",
                table: "CustomerTransactions",
                column: "TransCustomerDocSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTransactions_TransCustomerDocTypeId",
                table: "CustomerTransactions",
                column: "TransCustomerDocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTransactions_TransDate",
                table: "CustomerTransactions",
                column: "TransDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerTransactions");
        }
    }
}
