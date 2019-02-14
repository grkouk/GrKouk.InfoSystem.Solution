using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class TransactorTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransactorTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransDate = table.Column<DateTime>(nullable: false),
                    DocSeriesId = table.Column<int>(nullable: false),
                    DocTypeId = table.Column<int>(nullable: false),
                    TransRefCode = table.Column<string>(nullable: true),
                    TransactorId = table.Column<int>(nullable: false),
                    SectionId = table.Column<int>(nullable: false),
                    CreatorId = table.Column<int>(nullable: false),
                    FiscalPeriodId = table.Column<int>(nullable: false),
                    FinancialAction = table.Column<int>(nullable: false),
                    FpaRate = table.Column<decimal>(nullable: false),
                    DiscountRate = table.Column<decimal>(nullable: false),
                    AmountFpa = table.Column<decimal>(nullable: false),
                    AmountNet = table.Column<decimal>(nullable: false),
                    AmountDiscount = table.Column<decimal>(nullable: false),
                    TransFpaAmount = table.Column<decimal>(nullable: false),
                    TransNetAmount = table.Column<decimal>(nullable: false),
                    TransDiscountAmount = table.Column<decimal>(nullable: false),
                    Etiology = table.Column<string>(maxLength: 500, nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactorTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactorTransactions_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactorTransactions_FiscalPeriods_FiscalPeriodId",
                        column: x => x.FiscalPeriodId,
                        principalTable: "FiscalPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactorTransactions_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactorTransactions_Transactors_TransactorId",
                        column: x => x.TransactorId,
                        principalTable: "Transactors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransExpenseDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 20, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    InventoryAction = table.Column<int>(nullable: false),
                    InventoryValueAction = table.Column<int>(nullable: false),
                    DefaultDocSeriesId = table.Column<int>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransExpenseDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransExpenseDefs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactorTransactions_CompanyId",
                table: "TransactorTransactions",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactorTransactions_CreatorId",
                table: "TransactorTransactions",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactorTransactions_FiscalPeriodId",
                table: "TransactorTransactions",
                column: "FiscalPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactorTransactions_SectionId",
                table: "TransactorTransactions",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactorTransactions_TransDate",
                table: "TransactorTransactions",
                column: "TransDate");

            migrationBuilder.CreateIndex(
                name: "IX_TransactorTransactions_TransactorId",
                table: "TransactorTransactions",
                column: "TransactorId");

            migrationBuilder.CreateIndex(
                name: "IX_TransExpenseDefs_Code",
                table: "TransExpenseDefs",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TransExpenseDefs_CompanyId",
                table: "TransExpenseDefs",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactorTransactions");

            migrationBuilder.DropTable(
                name: "TransExpenseDefs");
        }
    }
}
