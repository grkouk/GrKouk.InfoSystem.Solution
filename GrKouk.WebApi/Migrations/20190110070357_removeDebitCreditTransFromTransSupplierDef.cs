using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class removeDebitCreditTransFromTransSupplierDef : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreditTransId",
                table: "TransSupplierDefs");

            migrationBuilder.DropColumn(
                name: "DebitTransId",
                table: "TransSupplierDefs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreditTransId",
                table: "TransSupplierDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DebitTransId",
                table: "TransSupplierDefs",
                nullable: false,
                defaultValue: 0);
        }
    }
}
