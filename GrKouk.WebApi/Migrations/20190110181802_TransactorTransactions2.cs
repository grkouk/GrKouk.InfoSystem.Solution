using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class TransactorTransactions2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DocTypeId",
                table: "TransactorTransactions",
                newName: "TransTransactorDocTypeId");

            migrationBuilder.RenameColumn(
                name: "DocSeriesId",
                table: "TransactorTransactions",
                newName: "TransTransactorDocSeriesId");

            migrationBuilder.CreateTable(
                name: "TransTransactorDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    FinancialTransAction = table.Column<int>(nullable: false),
                    TurnOverTransId = table.Column<int>(nullable: false),
                    DefaultDocSeriesId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransTransactorDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransTransactorDefs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransTransactorDefs_FinancialMovements_TurnOverTransId",
                        column: x => x.TurnOverTransId,
                        principalTable: "FinancialMovements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransTransactorDocTypeDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    TransTransactorDefId = table.Column<int>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransTransactorDocTypeDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransTransactorDocTypeDefs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransTransactorDocTypeDefs_TransTransactorDefs_TransTransactorDefId",
                        column: x => x.TransTransactorDefId,
                        principalTable: "TransTransactorDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransTransactorDocSeriesDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    TransTransactorDocTypeDefId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransTransactorDocSeriesDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransTransactorDocSeriesDefs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransTransactorDocSeriesDefs_TransTransactorDocTypeDefs_TransTransactorDocTypeDefId",
                        column: x => x.TransTransactorDocTypeDefId,
                        principalTable: "TransTransactorDocTypeDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactorTransactions_TransTransactorDocSeriesId",
                table: "TransactorTransactions",
                column: "TransTransactorDocSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactorTransactions_TransTransactorDocTypeId",
                table: "TransactorTransactions",
                column: "TransTransactorDocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransTransactorDefs_Code",
                table: "TransTransactorDefs",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TransTransactorDefs_CompanyId",
                table: "TransTransactorDefs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TransTransactorDefs_TurnOverTransId",
                table: "TransTransactorDefs",
                column: "TurnOverTransId");

            migrationBuilder.CreateIndex(
                name: "IX_TransTransactorDocSeriesDefs_Code",
                table: "TransTransactorDocSeriesDefs",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TransTransactorDocSeriesDefs_CompanyId",
                table: "TransTransactorDocSeriesDefs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TransTransactorDocSeriesDefs_TransTransactorDocTypeDefId",
                table: "TransTransactorDocSeriesDefs",
                column: "TransTransactorDocTypeDefId");

            migrationBuilder.CreateIndex(
                name: "IX_TransTransactorDocTypeDefs_Code",
                table: "TransTransactorDocTypeDefs",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TransTransactorDocTypeDefs_CompanyId",
                table: "TransTransactorDocTypeDefs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TransTransactorDocTypeDefs_TransTransactorDefId",
                table: "TransTransactorDocTypeDefs",
                column: "TransTransactorDefId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactorTransactions_TransTransactorDocSeriesDefs_TransTransactorDocSeriesId",
                table: "TransactorTransactions",
                column: "TransTransactorDocSeriesId",
                principalTable: "TransTransactorDocSeriesDefs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactorTransactions_TransTransactorDocTypeDefs_TransTransactorDocTypeId",
                table: "TransactorTransactions",
                column: "TransTransactorDocTypeId",
                principalTable: "TransTransactorDocTypeDefs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactorTransactions_TransTransactorDocSeriesDefs_TransTransactorDocSeriesId",
                table: "TransactorTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactorTransactions_TransTransactorDocTypeDefs_TransTransactorDocTypeId",
                table: "TransactorTransactions");

            migrationBuilder.DropTable(
                name: "TransTransactorDocSeriesDefs");

            migrationBuilder.DropTable(
                name: "TransTransactorDocTypeDefs");

            migrationBuilder.DropTable(
                name: "TransTransactorDefs");

            migrationBuilder.DropIndex(
                name: "IX_TransactorTransactions_TransTransactorDocSeriesId",
                table: "TransactorTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TransactorTransactions_TransTransactorDocTypeId",
                table: "TransactorTransactions");

            migrationBuilder.RenameColumn(
                name: "TransTransactorDocTypeId",
                table: "TransactorTransactions",
                newName: "DocTypeId");

            migrationBuilder.RenameColumn(
                name: "TransTransactorDocSeriesId",
                table: "TransactorTransactions",
                newName: "DocSeriesId");
        }
    }
}
