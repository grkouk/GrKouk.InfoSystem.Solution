using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class TransactorTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactors_TransactorType_TransactorTypeId",
                table: "Transactors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactorType",
                table: "TransactorType");

            migrationBuilder.RenameTable(
                name: "TransactorType",
                newName: "TransactorTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactorTypes",
                table: "TransactorTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactors_TransactorTypes_TransactorTypeId",
                table: "Transactors",
                column: "TransactorTypeId",
                principalTable: "TransactorTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactors_TransactorTypes_TransactorTypeId",
                table: "Transactors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactorTypes",
                table: "TransactorTypes");

            migrationBuilder.RenameTable(
                name: "TransactorTypes",
                newName: "TransactorType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactorType",
                table: "TransactorType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactors_TransactorType_TransactorTypeId",
                table: "Transactors",
                column: "TransactorTypeId",
                principalTable: "TransactorType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
