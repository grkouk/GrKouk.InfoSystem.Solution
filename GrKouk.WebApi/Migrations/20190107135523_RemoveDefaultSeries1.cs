using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class RemoveDefaultSeries1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransSupplierDefs_TransSupplierDocSeriesDefs_TransSupplierDefaultDocSeriesId",
                table: "TransSupplierDefs");

            migrationBuilder.DropIndex(
                name: "IX_TransSupplierDefs_TransSupplierDefaultDocSeriesId",
                table: "TransSupplierDefs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TransSupplierDefs_TransSupplierDefaultDocSeriesId",
                table: "TransSupplierDefs",
                column: "TransSupplierDefaultDocSeriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransSupplierDefs_TransSupplierDocSeriesDefs_TransSupplierDefaultDocSeriesId",
                table: "TransSupplierDefs",
                column: "TransSupplierDefaultDocSeriesId",
                principalTable: "TransSupplierDocSeriesDefs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
