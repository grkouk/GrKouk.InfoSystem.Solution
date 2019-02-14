using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class RemoveSupplierAndCustomerRelatedEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuyDocTypeDefs_TransSupplierDefs_TransSupplierDefId",
                table: "BuyDocTypeDefs");

            migrationBuilder.DropTable(
                name: "CustomerTransactions");

            migrationBuilder.DropTable(
                name: "SupplierTransactions");

            migrationBuilder.DropTable(
                name: "TransCustomerDocSeriesDefs");

            migrationBuilder.DropTable(
                name: "TransSupplierDocSeriesDefs");

            migrationBuilder.DropTable(
                name: "TransCustomerDocTypeDefs");

            migrationBuilder.DropTable(
                name: "TransSupplierDocTypeDefs");

            migrationBuilder.DropTable(
                name: "TransCustomerDefs");

            migrationBuilder.DropTable(
                name: "TransSupplierDefs");

            migrationBuilder.DropIndex(
                name: "IX_BuyDocTypeDefs_TransSupplierDefId",
                table: "BuyDocTypeDefs");

            migrationBuilder.DropColumn(
                name: "TransSupplierDefId",
                table: "BuyDocTypeDefs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TransSupplierDefId",
                table: "BuyDocTypeDefs",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TransCustomerDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(maxLength: 15, nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    DefaultDocSeriesId = table.Column<int>(nullable: true),
                    FinancialTransAction = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    TurnOverTransId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransCustomerDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransCustomerDefs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransCustomerDefs_FinancialMovements_TurnOverTransId",
                        column: x => x.TurnOverTransId,
                        principalTable: "FinancialMovements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransSupplierDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(maxLength: 15, nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    DefaultDocSeriesId = table.Column<int>(nullable: false),
                    FinancialAction = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    TurnOverTransId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransSupplierDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransSupplierDefs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransSupplierDefs_FinancialMovements_TurnOverTransId",
                        column: x => x.TurnOverTransId,
                        principalTable: "FinancialMovements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransCustomerDocTypeDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(maxLength: 15, nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    TransCustomerDefId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransCustomerDocTypeDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransCustomerDocTypeDefs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransCustomerDocTypeDefs_TransCustomerDefs_TransCustomerDefId",
                        column: x => x.TransCustomerDefId,
                        principalTable: "TransCustomerDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransSupplierDocTypeDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(maxLength: 15, nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    TransSupplierDefId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransSupplierDocTypeDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransSupplierDocTypeDefs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransSupplierDocTypeDefs_TransSupplierDefs_TransSupplierDefId",
                        column: x => x.TransSupplierDefId,
                        principalTable: "TransSupplierDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransCustomerDocSeriesDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(maxLength: 15, nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    TransCustomerDocTypeDefId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransCustomerDocSeriesDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransCustomerDocSeriesDefs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransCustomerDocSeriesDefs_TransCustomerDocTypeDefs_TransCustomerDocTypeDefId",
                        column: x => x.TransCustomerDocTypeDefId,
                        principalTable: "TransCustomerDocTypeDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransSupplierDocSeriesDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(maxLength: 15, nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    TransSupplierDocTypeDefId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransSupplierDocSeriesDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransSupplierDocSeriesDefs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransSupplierDocSeriesDefs_TransSupplierDocTypeDefs_TransSupplierDocTypeDefId",
                        column: x => x.TransSupplierDocTypeDefId,
                        principalTable: "TransSupplierDocTypeDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AmountDiscount = table.Column<decimal>(nullable: false),
                    AmountFpa = table.Column<decimal>(nullable: false),
                    AmountNet = table.Column<decimal>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    CreatorId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    DiscountRate = table.Column<decimal>(nullable: false),
                    Etiology = table.Column<string>(maxLength: 500, nullable: true),
                    FinancialAction = table.Column<int>(nullable: false),
                    FiscalPeriodId = table.Column<int>(nullable: false),
                    FpaRate = table.Column<decimal>(nullable: false),
                    SectionId = table.Column<int>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    TransCustomerDocSeriesId = table.Column<int>(nullable: false),
                    TransCustomerDocTypeId = table.Column<int>(nullable: false),
                    TransDate = table.Column<DateTime>(nullable: false),
                    TransRefCode = table.Column<string>(nullable: true),
                    TransactionType = table.Column<int>(nullable: false)
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
                        name: "FK_CustomerTransactions_FiscalPeriods_FiscalPeriodId",
                        column: x => x.FiscalPeriodId,
                        principalTable: "FiscalPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerTransactions_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
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

            migrationBuilder.CreateTable(
                name: "SupplierTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AmountDiscount = table.Column<decimal>(nullable: false),
                    AmountFpa = table.Column<decimal>(nullable: false),
                    AmountNet = table.Column<decimal>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    CreatorId = table.Column<int>(nullable: false),
                    DiscountRate = table.Column<decimal>(nullable: false),
                    Etiology = table.Column<string>(maxLength: 500, nullable: true),
                    FinancialAction = table.Column<int>(nullable: false),
                    FiscalPeriodId = table.Column<int>(nullable: false),
                    FpaRate = table.Column<decimal>(nullable: false),
                    SectionId = table.Column<int>(nullable: false),
                    SupplierId = table.Column<int>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    TransDate = table.Column<DateTime>(nullable: false),
                    TransRefCode = table.Column<string>(nullable: true),
                    TransSupplierDocSeriesId = table.Column<int>(nullable: false),
                    TransSupplierDocTypeId = table.Column<int>(nullable: false),
                    TransactionType = table.Column<int>(nullable: false)
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
                        name: "FK_SupplierTransactions_FiscalPeriods_FiscalPeriodId",
                        column: x => x.FiscalPeriodId,
                        principalTable: "FiscalPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupplierTransactions_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
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

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocTypeDefs_TransSupplierDefId",
                table: "BuyDocTypeDefs",
                column: "TransSupplierDefId");

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
                name: "IX_TransCustomerDefs_Code",
                table: "TransCustomerDefs",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TransCustomerDefs_CompanyId",
                table: "TransCustomerDefs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TransCustomerDefs_TurnOverTransId",
                table: "TransCustomerDefs",
                column: "TurnOverTransId");

            migrationBuilder.CreateIndex(
                name: "IX_TransCustomerDocSeriesDefs_Code",
                table: "TransCustomerDocSeriesDefs",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TransCustomerDocSeriesDefs_CompanyId",
                table: "TransCustomerDocSeriesDefs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TransCustomerDocSeriesDefs_TransCustomerDocTypeDefId",
                table: "TransCustomerDocSeriesDefs",
                column: "TransCustomerDocTypeDefId");

            migrationBuilder.CreateIndex(
                name: "IX_TransCustomerDocTypeDefs_Code",
                table: "TransCustomerDocTypeDefs",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TransCustomerDocTypeDefs_CompanyId",
                table: "TransCustomerDocTypeDefs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TransCustomerDocTypeDefs_TransCustomerDefId",
                table: "TransCustomerDocTypeDefs",
                column: "TransCustomerDefId");

            migrationBuilder.CreateIndex(
                name: "IX_TransSupplierDefs_Code",
                table: "TransSupplierDefs",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TransSupplierDefs_CompanyId",
                table: "TransSupplierDefs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TransSupplierDefs_DefaultDocSeriesId",
                table: "TransSupplierDefs",
                column: "DefaultDocSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_TransSupplierDefs_TurnOverTransId",
                table: "TransSupplierDefs",
                column: "TurnOverTransId");

            migrationBuilder.CreateIndex(
                name: "IX_TransSupplierDocSeriesDefs_Code",
                table: "TransSupplierDocSeriesDefs",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TransSupplierDocSeriesDefs_CompanyId",
                table: "TransSupplierDocSeriesDefs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TransSupplierDocSeriesDefs_TransSupplierDocTypeDefId",
                table: "TransSupplierDocSeriesDefs",
                column: "TransSupplierDocTypeDefId");

            migrationBuilder.CreateIndex(
                name: "IX_TransSupplierDocTypeDefs_Code",
                table: "TransSupplierDocTypeDefs",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TransSupplierDocTypeDefs_CompanyId",
                table: "TransSupplierDocTypeDefs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TransSupplierDocTypeDefs_TransSupplierDefId",
                table: "TransSupplierDocTypeDefs",
                column: "TransSupplierDefId");

            migrationBuilder.AddForeignKey(
                name: "FK_BuyDocTypeDefs_TransSupplierDefs_TransSupplierDefId",
                table: "BuyDocTypeDefs",
                column: "TransSupplierDefId",
                principalTable: "TransSupplierDefs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
