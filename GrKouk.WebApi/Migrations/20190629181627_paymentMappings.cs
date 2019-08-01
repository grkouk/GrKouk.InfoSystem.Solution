using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class paymentMappings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BuyDocTransPaymentMappings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BuyDocumentId = table.Column<int>(nullable: false),
                    TransactorTransactionId = table.Column<int>(nullable: false),
                    AmountUsed = table.Column<decimal>(type: "decimal(18, 4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyDocTransPaymentMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuyDocTransPaymentMappings_BuyDocuments_BuyDocumentId",
                        column: x => x.BuyDocumentId,
                        principalTable: "BuyDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuyDocTransPaymentMappings_TransactorTransactions_TransactorTransactionId",
                        column: x => x.TransactorTransactionId,
                        principalTable: "TransactorTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SellDocTransPaymentMappings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SellDocumentId = table.Column<int>(nullable: false),
                    TransactorTransactionId = table.Column<int>(nullable: false),
                    AmountUsed = table.Column<decimal>(type: "decimal(18, 4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellDocTransPaymentMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellDocTransPaymentMappings_SellDocuments_SellDocumentId",
                        column: x => x.SellDocumentId,
                        principalTable: "SellDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellDocTransPaymentMappings_TransactorTransactions_TransactorTransactionId",
                        column: x => x.TransactorTransactionId,
                        principalTable: "TransactorTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocTransPaymentMappings_TransactorTransactionId",
                table: "BuyDocTransPaymentMappings",
                column: "TransactorTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocTransPaymentMappings_BuyDocumentId_TransactorTransactionId",
                table: "BuyDocTransPaymentMappings",
                columns: new[] { "BuyDocumentId", "TransactorTransactionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SellDocTransPaymentMappings_TransactorTransactionId",
                table: "SellDocTransPaymentMappings",
                column: "TransactorTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_SellDocTransPaymentMappings_SellDocumentId_TransactorTransactionId",
                table: "SellDocTransPaymentMappings",
                columns: new[] { "SellDocumentId", "TransactorTransactionId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuyDocTransPaymentMappings");

            migrationBuilder.DropTable(
                name: "SellDocTransPaymentMappings");
        }
    }
}
