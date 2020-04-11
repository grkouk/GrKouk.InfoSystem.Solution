using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class r2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecurringTransDoc_Companies_CompanyId",
                table: "RecurringTransDoc");

            migrationBuilder.DropForeignKey(
                name: "FK_RecurringTransDoc_PaymentMethods_PaymentMethodId",
                table: "RecurringTransDoc");

            migrationBuilder.DropForeignKey(
                name: "FK_RecurringTransDoc_Transactors_TransactorId",
                table: "RecurringTransDoc");

            migrationBuilder.DropForeignKey(
                name: "FK_RecurringTransDocLine_RecurringTransDoc_RecurringTransDocId",
                table: "RecurringTransDocLine");

            migrationBuilder.DropForeignKey(
                name: "FK_RecurringTransDocLine_WarehouseItems_WarehouseItemId",
                table: "RecurringTransDocLine");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecurringTransDocLine",
                table: "RecurringTransDocLine");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecurringTransDoc",
                table: "RecurringTransDoc");

            migrationBuilder.RenameTable(
                name: "RecurringTransDocLine",
                newName: "RecurringTransDocLines");

            migrationBuilder.RenameTable(
                name: "RecurringTransDoc",
                newName: "RecurringTransDocs");

            migrationBuilder.RenameIndex(
                name: "IX_RecurringTransDocLine_WarehouseItemId",
                table: "RecurringTransDocLines",
                newName: "IX_RecurringTransDocLines_WarehouseItemId");

            migrationBuilder.RenameIndex(
                name: "IX_RecurringTransDocLine_RecurringTransDocId",
                table: "RecurringTransDocLines",
                newName: "IX_RecurringTransDocLines_RecurringTransDocId");

            migrationBuilder.RenameIndex(
                name: "IX_RecurringTransDoc_TransactorId",
                table: "RecurringTransDocs",
                newName: "IX_RecurringTransDocs_TransactorId");

            migrationBuilder.RenameIndex(
                name: "IX_RecurringTransDoc_PaymentMethodId",
                table: "RecurringTransDocs",
                newName: "IX_RecurringTransDocs_PaymentMethodId");

            migrationBuilder.RenameIndex(
                name: "IX_RecurringTransDoc_NextTransDate",
                table: "RecurringTransDocs",
                newName: "IX_RecurringTransDocs_NextTransDate");

            migrationBuilder.RenameIndex(
                name: "IX_RecurringTransDoc_CompanyId",
                table: "RecurringTransDocs",
                newName: "IX_RecurringTransDocs_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecurringTransDocLines",
                table: "RecurringTransDocLines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecurringTransDocs",
                table: "RecurringTransDocs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringTransDocLines_RecurringTransDocs_RecurringTransDocId",
                table: "RecurringTransDocLines",
                column: "RecurringTransDocId",
                principalTable: "RecurringTransDocs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringTransDocLines_WarehouseItems_WarehouseItemId",
                table: "RecurringTransDocLines",
                column: "WarehouseItemId",
                principalTable: "WarehouseItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringTransDocs_Companies_CompanyId",
                table: "RecurringTransDocs",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringTransDocs_PaymentMethods_PaymentMethodId",
                table: "RecurringTransDocs",
                column: "PaymentMethodId",
                principalTable: "PaymentMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringTransDocs_Transactors_TransactorId",
                table: "RecurringTransDocs",
                column: "TransactorId",
                principalTable: "Transactors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecurringTransDocLines_RecurringTransDocs_RecurringTransDocId",
                table: "RecurringTransDocLines");

            migrationBuilder.DropForeignKey(
                name: "FK_RecurringTransDocLines_WarehouseItems_WarehouseItemId",
                table: "RecurringTransDocLines");

            migrationBuilder.DropForeignKey(
                name: "FK_RecurringTransDocs_Companies_CompanyId",
                table: "RecurringTransDocs");

            migrationBuilder.DropForeignKey(
                name: "FK_RecurringTransDocs_PaymentMethods_PaymentMethodId",
                table: "RecurringTransDocs");

            migrationBuilder.DropForeignKey(
                name: "FK_RecurringTransDocs_Transactors_TransactorId",
                table: "RecurringTransDocs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecurringTransDocs",
                table: "RecurringTransDocs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecurringTransDocLines",
                table: "RecurringTransDocLines");

            migrationBuilder.RenameTable(
                name: "RecurringTransDocs",
                newName: "RecurringTransDoc");

            migrationBuilder.RenameTable(
                name: "RecurringTransDocLines",
                newName: "RecurringTransDocLine");

            migrationBuilder.RenameIndex(
                name: "IX_RecurringTransDocs_TransactorId",
                table: "RecurringTransDoc",
                newName: "IX_RecurringTransDoc_TransactorId");

            migrationBuilder.RenameIndex(
                name: "IX_RecurringTransDocs_PaymentMethodId",
                table: "RecurringTransDoc",
                newName: "IX_RecurringTransDoc_PaymentMethodId");

            migrationBuilder.RenameIndex(
                name: "IX_RecurringTransDocs_NextTransDate",
                table: "RecurringTransDoc",
                newName: "IX_RecurringTransDoc_NextTransDate");

            migrationBuilder.RenameIndex(
                name: "IX_RecurringTransDocs_CompanyId",
                table: "RecurringTransDoc",
                newName: "IX_RecurringTransDoc_CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_RecurringTransDocLines_WarehouseItemId",
                table: "RecurringTransDocLine",
                newName: "IX_RecurringTransDocLine_WarehouseItemId");

            migrationBuilder.RenameIndex(
                name: "IX_RecurringTransDocLines_RecurringTransDocId",
                table: "RecurringTransDocLine",
                newName: "IX_RecurringTransDocLine_RecurringTransDocId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecurringTransDoc",
                table: "RecurringTransDoc",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecurringTransDocLine",
                table: "RecurringTransDocLine",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringTransDoc_Companies_CompanyId",
                table: "RecurringTransDoc",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringTransDoc_PaymentMethods_PaymentMethodId",
                table: "RecurringTransDoc",
                column: "PaymentMethodId",
                principalTable: "PaymentMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringTransDoc_Transactors_TransactorId",
                table: "RecurringTransDoc",
                column: "TransactorId",
                principalTable: "Transactors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringTransDocLine_RecurringTransDoc_RecurringTransDocId",
                table: "RecurringTransDocLine",
                column: "RecurringTransDocId",
                principalTable: "RecurringTransDoc",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringTransDocLine_WarehouseItems_WarehouseItemId",
                table: "RecurringTransDocLine",
                column: "WarehouseItemId",
                principalTable: "WarehouseItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
