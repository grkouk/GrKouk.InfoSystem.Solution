using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class RecurringTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecurringTransDoc",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RecurringFrequency = table.Column<string>(maxLength: 2, nullable: true),
                    RecurringDocType = table.Column<int>(nullable: false),
                    NextTransDate = table.Column<DateTime>(nullable: false),
                    TransRefCode = table.Column<string>(nullable: true),
                    SectionId = table.Column<int>(nullable: false),
                    TransactorId = table.Column<int>(nullable: false),
                    DocSeriesId = table.Column<int>(nullable: false),
                    DocTypeId = table.Column<int>(nullable: false),
                    AmountFpa = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    AmountNet = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    AmountDiscount = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    Etiology = table.Column<string>(maxLength: 500, nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    PaymentMethodId = table.Column<int>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurringTransDoc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecurringTransDoc_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecurringTransDoc_PaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecurringTransDoc_Transactors_TransactorId",
                        column: x => x.TransactorId,
                        principalTable: "Transactors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecurringTransDocLine",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RecurringTransDocId = table.Column<int>(nullable: false),
                    WarehouseItemId = table.Column<int>(nullable: false),
                    PrimaryUnitId = table.Column<int>(nullable: false),
                    SecondaryUnitId = table.Column<int>(nullable: false),
                    Factor = table.Column<float>(nullable: false),
                    Quontity1 = table.Column<double>(nullable: false),
                    Quontity2 = table.Column<double>(nullable: false),
                    FpaRate = table.Column<decimal>(nullable: false),
                    DiscountRate = table.Column<decimal>(nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    UnitExpenses = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    AmountFpa = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    AmountNet = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    AmountDiscount = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    Etiology = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurringTransDocLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecurringTransDocLine_RecurringTransDoc_RecurringTransDocId",
                        column: x => x.RecurringTransDocId,
                        principalTable: "RecurringTransDoc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecurringTransDocLine_WarehouseItems_WarehouseItemId",
                        column: x => x.WarehouseItemId,
                        principalTable: "WarehouseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecurringTransDoc_CompanyId",
                table: "RecurringTransDoc",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringTransDoc_NextTransDate",
                table: "RecurringTransDoc",
                column: "NextTransDate");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringTransDoc_PaymentMethodId",
                table: "RecurringTransDoc",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringTransDoc_TransactorId",
                table: "RecurringTransDoc",
                column: "TransactorId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringTransDocLine_RecurringTransDocId",
                table: "RecurringTransDocLine",
                column: "RecurringTransDocId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringTransDocLine_WarehouseItemId",
                table: "RecurringTransDocLine",
                column: "WarehouseItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecurringTransDocLine");

            migrationBuilder.DropTable(
                name: "RecurringTransDoc");
        }
    }
}
