using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class FinancialDiaryEntities1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinancialMovements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialMovements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FpaKategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Rate = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FpaKategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransSupplierDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    CreditTransId = table.Column<int>(nullable: false),
                    DebitTransId = table.Column<int>(nullable: false),
                    TurnOverTransId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false)
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
                        name: "FK_TransSupplierDefs_FinancialMovements_CreditTransId",
                        column: x => x.CreditTransId,
                        principalTable: "FinancialMovements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransSupplierDefs_FinancialMovements_DebitTransId",
                        column: x => x.DebitTransId,
                        principalTable: "FinancialMovements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransSupplierDefs_FinancialMovements_TurnOverTransId",
                        column: x => x.TurnOverTransId,
                        principalTable: "FinancialMovements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransWarehouseDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    VolImportsTransId = table.Column<int>(nullable: false),
                    VolExportsTransId = table.Column<int>(nullable: false),
                    VolInvoicedExportsTransId = table.Column<int>(nullable: false),
                    VolInvoicedImportsTransId = table.Column<int>(nullable: false),
                    AmtImportsTransId = table.Column<int>(nullable: false),
                    AmtExportsTransId = table.Column<int>(nullable: false),
                    AmtInvoicedExportsTransId = table.Column<int>(nullable: false),
                    AmtInvoicedImportsTransId = table.Column<int>(nullable: false),
                    VolBuyTransId = table.Column<int>(nullable: false),
                    AmtBuyTransId = table.Column<int>(nullable: false),
                    VolSellTransId = table.Column<int>(nullable: false),
                    AmtSellTransId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransWarehouseDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransWarehouseDefs_FinancialMovements_AmtBuyTransId",
                        column: x => x.AmtBuyTransId,
                        principalTable: "FinancialMovements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransWarehouseDefs_FinancialMovements_AmtExportsTransId",
                        column: x => x.AmtExportsTransId,
                        principalTable: "FinancialMovements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransWarehouseDefs_FinancialMovements_AmtImportsTransId",
                        column: x => x.AmtImportsTransId,
                        principalTable: "FinancialMovements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransWarehouseDefs_FinancialMovements_AmtInvoicedExportsTransId",
                        column: x => x.AmtInvoicedExportsTransId,
                        principalTable: "FinancialMovements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransWarehouseDefs_FinancialMovements_AmtInvoicedImportsTransId",
                        column: x => x.AmtInvoicedImportsTransId,
                        principalTable: "FinancialMovements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransWarehouseDefs_FinancialMovements_AmtSellTransId",
                        column: x => x.AmtSellTransId,
                        principalTable: "FinancialMovements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransWarehouseDefs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransWarehouseDefs_FinancialMovements_VolBuyTransId",
                        column: x => x.VolBuyTransId,
                        principalTable: "FinancialMovements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransWarehouseDefs_FinancialMovements_VolExportsTransId",
                        column: x => x.VolExportsTransId,
                        principalTable: "FinancialMovements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransWarehouseDefs_FinancialMovements_VolImportsTransId",
                        column: x => x.VolImportsTransId,
                        principalTable: "FinancialMovements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransWarehouseDefs_FinancialMovements_VolInvoicedExportsTransId",
                        column: x => x.VolInvoicedExportsTransId,
                        principalTable: "FinancialMovements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransWarehouseDefs_FinancialMovements_VolInvoicedImportsTransId",
                        column: x => x.VolInvoicedImportsTransId,
                        principalTable: "FinancialMovements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransWarehouseDefs_FinancialMovements_VolSellTransId",
                        column: x => x.VolSellTransId,
                        principalTable: "FinancialMovements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransSupplierDocTypeDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    TransSupplierDefId = table.Column<int>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false)
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
                name: "TransWarehouseDocTypeDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    TransWarehouseDefId = table.Column<int>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransWarehouseDocTypeDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransWarehouseDocTypeDefs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransWarehouseDocTypeDefs_TransWarehouseDefs_TransWarehouseDefId",
                        column: x => x.TransWarehouseDefId,
                        principalTable: "TransWarehouseDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransSupplierDocSeriesDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    TransSupplierDocTypeDefId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false)
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
                name: "TransWarehouseDocSeriesDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    TransWarehouseDocTypeDefId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransWarehouseDocSeriesDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransWarehouseDocSeriesDefs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransWarehouseDocSeriesDefs_TransWarehouseDocTypeDefs_TransWarehouseDocTypeDefId",
                        column: x => x.TransWarehouseDocTypeDefId,
                        principalTable: "TransWarehouseDocTypeDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinancialMovements_Code",
                table: "FinancialMovements",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FpaKategories_Code",
                table: "FpaKategories",
                column: "Code",
                unique: true);

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
                name: "IX_TransSupplierDefs_CreditTransId",
                table: "TransSupplierDefs",
                column: "CreditTransId");

            migrationBuilder.CreateIndex(
                name: "IX_TransSupplierDefs_DebitTransId",
                table: "TransSupplierDefs",
                column: "DebitTransId");

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

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDefs_AmtBuyTransId",
                table: "TransWarehouseDefs",
                column: "AmtBuyTransId");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDefs_AmtExportsTransId",
                table: "TransWarehouseDefs",
                column: "AmtExportsTransId");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDefs_AmtImportsTransId",
                table: "TransWarehouseDefs",
                column: "AmtImportsTransId");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDefs_AmtInvoicedExportsTransId",
                table: "TransWarehouseDefs",
                column: "AmtInvoicedExportsTransId");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDefs_AmtInvoicedImportsTransId",
                table: "TransWarehouseDefs",
                column: "AmtInvoicedImportsTransId");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDefs_AmtSellTransId",
                table: "TransWarehouseDefs",
                column: "AmtSellTransId");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDefs_Code",
                table: "TransWarehouseDefs",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDefs_CompanyId",
                table: "TransWarehouseDefs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDefs_VolBuyTransId",
                table: "TransWarehouseDefs",
                column: "VolBuyTransId");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDefs_VolExportsTransId",
                table: "TransWarehouseDefs",
                column: "VolExportsTransId");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDefs_VolImportsTransId",
                table: "TransWarehouseDefs",
                column: "VolImportsTransId");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDefs_VolInvoicedExportsTransId",
                table: "TransWarehouseDefs",
                column: "VolInvoicedExportsTransId");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDefs_VolInvoicedImportsTransId",
                table: "TransWarehouseDefs",
                column: "VolInvoicedImportsTransId");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDefs_VolSellTransId",
                table: "TransWarehouseDefs",
                column: "VolSellTransId");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDocSeriesDefs_Code",
                table: "TransWarehouseDocSeriesDefs",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDocSeriesDefs_CompanyId",
                table: "TransWarehouseDocSeriesDefs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDocSeriesDefs_TransWarehouseDocTypeDefId",
                table: "TransWarehouseDocSeriesDefs",
                column: "TransWarehouseDocTypeDefId");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDocTypeDefs_Code",
                table: "TransWarehouseDocTypeDefs",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDocTypeDefs_CompanyId",
                table: "TransWarehouseDocTypeDefs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDocTypeDefs_TransWarehouseDefId",
                table: "TransWarehouseDocTypeDefs",
                column: "TransWarehouseDefId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FpaKategories");

            migrationBuilder.DropTable(
                name: "TransSupplierDocSeriesDefs");

            migrationBuilder.DropTable(
                name: "TransWarehouseDocSeriesDefs");

            migrationBuilder.DropTable(
                name: "TransSupplierDocTypeDefs");

            migrationBuilder.DropTable(
                name: "TransWarehouseDocTypeDefs");

            migrationBuilder.DropTable(
                name: "TransSupplierDefs");

            migrationBuilder.DropTable(
                name: "TransWarehouseDefs");

            migrationBuilder.DropTable(
                name: "FinancialMovements");
        }
    }
}
