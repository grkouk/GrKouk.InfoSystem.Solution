using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class TransactionEntiiesChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "FpaRate",
                table: "WarehouseTransactions",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountRate",
                table: "WarehouseTransactions",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "WarehouseTransactions",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "FinancialTransType",
                table: "TransCustomerDefs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "FpaRate",
                table: "SupplierTransactions",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountRate",
                table: "SupplierTransactions",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "FpaRate",
                table: "CustomerTransactions",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountRate",
                table: "CustomerTransactions",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "FinancialAction",
                table: "CustomerTransactions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "FpaRate",
                table: "BuyMaterialsDocLines",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountRate",
                table: "BuyMaterialsDocLines",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AddColumn<decimal>(
                name: "AmountDiscount",
                table: "BuyMaterialsDocLines",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "BuyMaterialsDocLines",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountRate",
                table: "WarehouseTransactions");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "WarehouseTransactions");

            migrationBuilder.DropColumn(
                name: "FinancialTransType",
                table: "TransCustomerDefs");

            migrationBuilder.DropColumn(
                name: "DiscountRate",
                table: "SupplierTransactions");

            migrationBuilder.DropColumn(
                name: "DiscountRate",
                table: "CustomerTransactions");

            migrationBuilder.DropColumn(
                name: "FinancialAction",
                table: "CustomerTransactions");

            migrationBuilder.DropColumn(
                name: "AmountDiscount",
                table: "BuyMaterialsDocLines");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "BuyMaterialsDocLines");

            migrationBuilder.AlterColumn<float>(
                name: "FpaRate",
                table: "WarehouseTransactions",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<float>(
                name: "FpaRate",
                table: "SupplierTransactions",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<float>(
                name: "FpaRate",
                table: "CustomerTransactions",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<float>(
                name: "FpaRate",
                table: "BuyMaterialsDocLines",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<float>(
                name: "DiscountRate",
                table: "BuyMaterialsDocLines",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
