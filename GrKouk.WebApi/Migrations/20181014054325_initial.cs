using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CostCentres",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostCentres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinTransCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinTransCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RevenueCentres",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevenueCentres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactorType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsSystem = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(maxLength: 15, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactorType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transactors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Address = table.Column<string>(maxLength: 200, nullable: true),
                    City = table.Column<string>(maxLength: 50, nullable: true),
                    Zip = table.Column<int>(nullable: true),
                    PhoneWork = table.Column<string>(maxLength: 200, nullable: true),
                    PhoneMobile = table.Column<string>(maxLength: 200, nullable: true),
                    PhoneFax = table.Column<string>(maxLength: 200, nullable: true),
                    EMail = table.Column<string>(maxLength: 200, nullable: true),
                    TransactorTypeId = table.Column<int>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactors_TransactorType_TransactorTypeId",
                        column: x => x.TransactorTypeId,
                        principalTable: "TransactorType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FinDiaryTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    ReferenceCode = table.Column<string>(nullable: true),
                    TransactorId = table.Column<int>(nullable: false),
                    FinTransCategoryId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    CostCentreId = table.Column<int>(nullable: false),
                    RevenueCentreId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Kind = table.Column<int>(nullable: false),
                    AmountFpa = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountNet = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinDiaryTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinDiaryTransactions_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FinDiaryTransactions_CostCentres_CostCentreId",
                        column: x => x.CostCentreId,
                        principalTable: "CostCentres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FinDiaryTransactions_FinTransCategories_FinTransCategoryId",
                        column: x => x.FinTransCategoryId,
                        principalTable: "FinTransCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FinDiaryTransactions_RevenueCentres_RevenueCentreId",
                        column: x => x.RevenueCentreId,
                        principalTable: "RevenueCentres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FinDiaryTransactions_Transactors_TransactorId",
                        column: x => x.TransactorId,
                        principalTable: "Transactors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinDiaryTransactions_CompanyId",
                table: "FinDiaryTransactions",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_FinDiaryTransactions_CostCentreId",
                table: "FinDiaryTransactions",
                column: "CostCentreId");

            migrationBuilder.CreateIndex(
                name: "IX_FinDiaryTransactions_FinTransCategoryId",
                table: "FinDiaryTransactions",
                column: "FinTransCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FinDiaryTransactions_RevenueCentreId",
                table: "FinDiaryTransactions",
                column: "RevenueCentreId");

            migrationBuilder.CreateIndex(
                name: "IX_FinDiaryTransactions_TransactionDate",
                table: "FinDiaryTransactions",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_FinDiaryTransactions_TransactorId",
                table: "FinDiaryTransactions",
                column: "TransactorId");

            migrationBuilder.CreateIndex(
                name: "IX_FinTransCategories_Code",
                table: "FinTransCategories",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Transactors_Code",
                table: "Transactors",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Transactors_TransactorTypeId",
                table: "Transactors",
                column: "TransactorTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinDiaryTransactions");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "CostCentres");

            migrationBuilder.DropTable(
                name: "FinTransCategories");

            migrationBuilder.DropTable(
                name: "RevenueCentres");

            migrationBuilder.DropTable(
                name: "Transactors");

            migrationBuilder.DropTable(
                name: "TransactorType");
        }
    }
}
