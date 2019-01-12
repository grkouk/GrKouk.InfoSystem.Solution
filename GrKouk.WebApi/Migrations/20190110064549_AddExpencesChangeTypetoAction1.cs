using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class AddExpencesChangeTypetoAction1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreditTransId",
                table: "TransCustomerDefs");

            migrationBuilder.DropColumn(
                name: "DebitTransId",
                table: "TransCustomerDefs");

            migrationBuilder.DropColumn(
                name: "FinancialTransType",
                table: "TransCustomerDefs");

            migrationBuilder.RenameColumn(
                name: "TransCustomerDefaultDocSeriesId",
                table: "TransCustomerDefs",
                newName: "DefaultDocSeriesId");

            migrationBuilder.AddColumn<int>(
                name: "FinancialTransAction",
                table: "TransCustomerDefs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinancialTransAction",
                table: "TransCustomerDefs");

            migrationBuilder.RenameColumn(
                name: "DefaultDocSeriesId",
                table: "TransCustomerDefs",
                newName: "TransCustomerDefaultDocSeriesId");

            migrationBuilder.AddColumn<int>(
                name: "CreditTransId",
                table: "TransCustomerDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DebitTransId",
                table: "TransCustomerDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FinancialTransType",
                table: "TransCustomerDefs",
                nullable: false,
                defaultValue: 0);
        }
    }
}
