using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class noFpaInTransactionEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTransactions_FpaKategories_FpaDefId",
                table: "CustomerTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierTransactions_FpaKategories_FpaDefId",
                table: "SupplierTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseTransactions_FpaKategories_FpaDefId",
                table: "WarehouseTransactions");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseTransactions_FpaDefId",
                table: "WarehouseTransactions");

            migrationBuilder.DropIndex(
                name: "IX_SupplierTransactions_FpaDefId",
                table: "SupplierTransactions");

            migrationBuilder.DropIndex(
                name: "IX_CustomerTransactions_FpaDefId",
                table: "CustomerTransactions");

            migrationBuilder.DropColumn(
                name: "FpaDefId",
                table: "WarehouseTransactions");

            migrationBuilder.DropColumn(
                name: "FpaId",
                table: "WarehouseTransactions");

            migrationBuilder.DropColumn(
                name: "FpaDefId",
                table: "SupplierTransactions");

            migrationBuilder.DropColumn(
                name: "FpaDefId",
                table: "CustomerTransactions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FpaDefId",
                table: "WarehouseTransactions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FpaId",
                table: "WarehouseTransactions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FpaDefId",
                table: "SupplierTransactions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FpaDefId",
                table: "CustomerTransactions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTransactions_FpaDefId",
                table: "WarehouseTransactions",
                column: "FpaDefId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierTransactions_FpaDefId",
                table: "SupplierTransactions",
                column: "FpaDefId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTransactions_FpaDefId",
                table: "CustomerTransactions",
                column: "FpaDefId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTransactions_FpaKategories_FpaDefId",
                table: "CustomerTransactions",
                column: "FpaDefId",
                principalTable: "FpaKategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierTransactions_FpaKategories_FpaDefId",
                table: "SupplierTransactions",
                column: "FpaDefId",
                principalTable: "FpaKategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseTransactions_FpaKategories_FpaDefId",
                table: "WarehouseTransactions",
                column: "FpaDefId",
                principalTable: "FpaKategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
