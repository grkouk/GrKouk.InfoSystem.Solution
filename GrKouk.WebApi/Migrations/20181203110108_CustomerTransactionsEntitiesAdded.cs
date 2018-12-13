using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class CustomerTransactionsEntitiesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "FinancialMovements",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "TransCustomerDefs",
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
                    table.PrimaryKey("PK_TransCustomerDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransCustomerDefs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransCustomerDefs_FinancialMovements_CreditTransId",
                        column: x => x.CreditTransId,
                        principalTable: "FinancialMovements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransCustomerDefs_FinancialMovements_DebitTransId",
                        column: x => x.DebitTransId,
                        principalTable: "FinancialMovements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransCustomerDefs_FinancialMovements_TurnOverTransId",
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
                    Code = table.Column<string>(maxLength: 15, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    TransCustomerDefId = table.Column<int>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false)
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
                name: "TransCustomerDocSeriesDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    TransCustomerDocTypeDefId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false)
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
                name: "IX_TransCustomerDefs_CreditTransId",
                table: "TransCustomerDefs",
                column: "CreditTransId");

            migrationBuilder.CreateIndex(
                name: "IX_TransCustomerDefs_DebitTransId",
                table: "TransCustomerDefs",
                column: "DebitTransId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransCustomerDocSeriesDefs");

            migrationBuilder.DropTable(
                name: "TransCustomerDocTypeDefs");

            migrationBuilder.DropTable(
                name: "TransCustomerDefs");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "FinancialMovements",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 200,
                oldNullable: true);
        }
    }
}
