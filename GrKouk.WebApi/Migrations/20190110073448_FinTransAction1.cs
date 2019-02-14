using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class FinTransAction1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinancialTransType",
                table: "TransSupplierDefs");

            migrationBuilder.RenameColumn(
                name: "TransSupplierDefaultDocSeriesId",
                table: "TransSupplierDefs",
                newName: "DefaultDocSeriesId");

            migrationBuilder.RenameIndex(
                name: "IX_TransSupplierDefs_TransSupplierDefaultDocSeriesId",
                table: "TransSupplierDefs",
                newName: "IX_TransSupplierDefs_DefaultDocSeriesId");

            migrationBuilder.AddColumn<int>(
                name: "FinancialTransAction",
                table: "TransSupplierDefs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinancialTransAction",
                table: "TransSupplierDefs");

            migrationBuilder.RenameColumn(
                name: "DefaultDocSeriesId",
                table: "TransSupplierDefs",
                newName: "TransSupplierDefaultDocSeriesId");

            migrationBuilder.RenameIndex(
                name: "IX_TransSupplierDefs_DefaultDocSeriesId",
                table: "TransSupplierDefs",
                newName: "IX_TransSupplierDefs_TransSupplierDefaultDocSeriesId");

            migrationBuilder.AddColumn<int>(
                name: "FinancialTransType",
                table: "TransSupplierDefs",
                nullable: false,
                defaultValue: 0);
        }
    }
}
