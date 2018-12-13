using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class SectionsAndFiscalPeriodsEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuyMaterialsDocLines_buyMaterialsDocuments_BuyDocumentId",
                table: "BuyMaterialsDocLines");

            migrationBuilder.DropForeignKey(
                name: "FK_BuyMaterialsDocLines_buyMaterialsDocuments_BuyMaterialsDocumentId",
                table: "BuyMaterialsDocLines");

            migrationBuilder.DropForeignKey(
                name: "FK_buyMaterialsDocuments_Companies_CompanyId",
                table: "buyMaterialsDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_buyMaterialsDocuments_BuyDocSeriesDefs_DocSeriesId",
                table: "buyMaterialsDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_buyMaterialsDocuments_BuyDocTypeDefs_DocTypeId",
                table: "buyMaterialsDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_buyMaterialsDocuments_FiscalPeriod_FiscalPeriodId",
                table: "buyMaterialsDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_buyMaterialsDocuments_Section_SectionId",
                table: "buyMaterialsDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_buyMaterialsDocuments_Transactors_SupplierId",
                table: "buyMaterialsDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTransactions_FiscalPeriod_FiscalPeriodId",
                table: "CustomerTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTransactions_Section_SectionId",
                table: "CustomerTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierTransactions_FiscalPeriod_FiscalPeriodId",
                table: "SupplierTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierTransactions_Section_SectionId",
                table: "SupplierTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseTransactions_FiscalPeriod_FiscalPeriodId",
                table: "WarehouseTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseTransactions_Section_SectionId",
                table: "WarehouseTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_buyMaterialsDocuments",
                table: "buyMaterialsDocuments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Section",
                table: "Section");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FiscalPeriod",
                table: "FiscalPeriod");

            migrationBuilder.RenameTable(
                name: "buyMaterialsDocuments",
                newName: "BuyMaterialsDocuments");

            migrationBuilder.RenameTable(
                name: "Section",
                newName: "Sections");

            migrationBuilder.RenameTable(
                name: "FiscalPeriod",
                newName: "FiscalPeriods");

            migrationBuilder.RenameIndex(
                name: "IX_buyMaterialsDocuments_TransDate",
                table: "BuyMaterialsDocuments",
                newName: "IX_BuyMaterialsDocuments_TransDate");

            migrationBuilder.RenameIndex(
                name: "IX_buyMaterialsDocuments_SupplierId",
                table: "BuyMaterialsDocuments",
                newName: "IX_BuyMaterialsDocuments_SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_buyMaterialsDocuments_SectionId",
                table: "BuyMaterialsDocuments",
                newName: "IX_BuyMaterialsDocuments_SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_buyMaterialsDocuments_FiscalPeriodId",
                table: "BuyMaterialsDocuments",
                newName: "IX_BuyMaterialsDocuments_FiscalPeriodId");

            migrationBuilder.RenameIndex(
                name: "IX_buyMaterialsDocuments_DocTypeId",
                table: "BuyMaterialsDocuments",
                newName: "IX_BuyMaterialsDocuments_DocTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_buyMaterialsDocuments_DocSeriesId",
                table: "BuyMaterialsDocuments",
                newName: "IX_BuyMaterialsDocuments_DocSeriesId");

            migrationBuilder.RenameIndex(
                name: "IX_buyMaterialsDocuments_CompanyId",
                table: "BuyMaterialsDocuments",
                newName: "IX_BuyMaterialsDocuments_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BuyMaterialsDocuments",
                table: "BuyMaterialsDocuments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sections",
                table: "Sections",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FiscalPeriods",
                table: "FiscalPeriods",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_Code",
                table: "Sections",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FiscalPeriods_Code",
                table: "FiscalPeriods",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_BuyMaterialsDocLines_BuyMaterialsDocuments_BuyDocumentId",
                table: "BuyMaterialsDocLines",
                column: "BuyDocumentId",
                principalTable: "BuyMaterialsDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BuyMaterialsDocLines_BuyMaterialsDocuments_BuyMaterialsDocumentId",
                table: "BuyMaterialsDocLines",
                column: "BuyMaterialsDocumentId",
                principalTable: "BuyMaterialsDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BuyMaterialsDocuments_Companies_CompanyId",
                table: "BuyMaterialsDocuments",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BuyMaterialsDocuments_BuyDocSeriesDefs_DocSeriesId",
                table: "BuyMaterialsDocuments",
                column: "DocSeriesId",
                principalTable: "BuyDocSeriesDefs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BuyMaterialsDocuments_BuyDocTypeDefs_DocTypeId",
                table: "BuyMaterialsDocuments",
                column: "DocTypeId",
                principalTable: "BuyDocTypeDefs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BuyMaterialsDocuments_FiscalPeriods_FiscalPeriodId",
                table: "BuyMaterialsDocuments",
                column: "FiscalPeriodId",
                principalTable: "FiscalPeriods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BuyMaterialsDocuments_Sections_SectionId",
                table: "BuyMaterialsDocuments",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BuyMaterialsDocuments_Transactors_SupplierId",
                table: "BuyMaterialsDocuments",
                column: "SupplierId",
                principalTable: "Transactors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTransactions_FiscalPeriods_FiscalPeriodId",
                table: "CustomerTransactions",
                column: "FiscalPeriodId",
                principalTable: "FiscalPeriods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTransactions_Sections_SectionId",
                table: "CustomerTransactions",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierTransactions_FiscalPeriods_FiscalPeriodId",
                table: "SupplierTransactions",
                column: "FiscalPeriodId",
                principalTable: "FiscalPeriods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierTransactions_Sections_SectionId",
                table: "SupplierTransactions",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseTransactions_FiscalPeriods_FiscalPeriodId",
                table: "WarehouseTransactions",
                column: "FiscalPeriodId",
                principalTable: "FiscalPeriods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseTransactions_Sections_SectionId",
                table: "WarehouseTransactions",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuyMaterialsDocLines_BuyMaterialsDocuments_BuyDocumentId",
                table: "BuyMaterialsDocLines");

            migrationBuilder.DropForeignKey(
                name: "FK_BuyMaterialsDocLines_BuyMaterialsDocuments_BuyMaterialsDocumentId",
                table: "BuyMaterialsDocLines");

            migrationBuilder.DropForeignKey(
                name: "FK_BuyMaterialsDocuments_Companies_CompanyId",
                table: "BuyMaterialsDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_BuyMaterialsDocuments_BuyDocSeriesDefs_DocSeriesId",
                table: "BuyMaterialsDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_BuyMaterialsDocuments_BuyDocTypeDefs_DocTypeId",
                table: "BuyMaterialsDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_BuyMaterialsDocuments_FiscalPeriods_FiscalPeriodId",
                table: "BuyMaterialsDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_BuyMaterialsDocuments_Sections_SectionId",
                table: "BuyMaterialsDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_BuyMaterialsDocuments_Transactors_SupplierId",
                table: "BuyMaterialsDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTransactions_FiscalPeriods_FiscalPeriodId",
                table: "CustomerTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTransactions_Sections_SectionId",
                table: "CustomerTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierTransactions_FiscalPeriods_FiscalPeriodId",
                table: "SupplierTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierTransactions_Sections_SectionId",
                table: "SupplierTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseTransactions_FiscalPeriods_FiscalPeriodId",
                table: "WarehouseTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseTransactions_Sections_SectionId",
                table: "WarehouseTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BuyMaterialsDocuments",
                table: "BuyMaterialsDocuments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sections",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Sections_Code",
                table: "Sections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FiscalPeriods",
                table: "FiscalPeriods");

            migrationBuilder.DropIndex(
                name: "IX_FiscalPeriods_Code",
                table: "FiscalPeriods");

            migrationBuilder.RenameTable(
                name: "BuyMaterialsDocuments",
                newName: "buyMaterialsDocuments");

            migrationBuilder.RenameTable(
                name: "Sections",
                newName: "Section");

            migrationBuilder.RenameTable(
                name: "FiscalPeriods",
                newName: "FiscalPeriod");

            migrationBuilder.RenameIndex(
                name: "IX_BuyMaterialsDocuments_TransDate",
                table: "buyMaterialsDocuments",
                newName: "IX_buyMaterialsDocuments_TransDate");

            migrationBuilder.RenameIndex(
                name: "IX_BuyMaterialsDocuments_SupplierId",
                table: "buyMaterialsDocuments",
                newName: "IX_buyMaterialsDocuments_SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_BuyMaterialsDocuments_SectionId",
                table: "buyMaterialsDocuments",
                newName: "IX_buyMaterialsDocuments_SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_BuyMaterialsDocuments_FiscalPeriodId",
                table: "buyMaterialsDocuments",
                newName: "IX_buyMaterialsDocuments_FiscalPeriodId");

            migrationBuilder.RenameIndex(
                name: "IX_BuyMaterialsDocuments_DocTypeId",
                table: "buyMaterialsDocuments",
                newName: "IX_buyMaterialsDocuments_DocTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_BuyMaterialsDocuments_DocSeriesId",
                table: "buyMaterialsDocuments",
                newName: "IX_buyMaterialsDocuments_DocSeriesId");

            migrationBuilder.RenameIndex(
                name: "IX_BuyMaterialsDocuments_CompanyId",
                table: "buyMaterialsDocuments",
                newName: "IX_buyMaterialsDocuments_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_buyMaterialsDocuments",
                table: "buyMaterialsDocuments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Section",
                table: "Section",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FiscalPeriod",
                table: "FiscalPeriod",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BuyMaterialsDocLines_buyMaterialsDocuments_BuyDocumentId",
                table: "BuyMaterialsDocLines",
                column: "BuyDocumentId",
                principalTable: "buyMaterialsDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BuyMaterialsDocLines_buyMaterialsDocuments_BuyMaterialsDocumentId",
                table: "BuyMaterialsDocLines",
                column: "BuyMaterialsDocumentId",
                principalTable: "buyMaterialsDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_buyMaterialsDocuments_Companies_CompanyId",
                table: "buyMaterialsDocuments",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_buyMaterialsDocuments_BuyDocSeriesDefs_DocSeriesId",
                table: "buyMaterialsDocuments",
                column: "DocSeriesId",
                principalTable: "BuyDocSeriesDefs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_buyMaterialsDocuments_BuyDocTypeDefs_DocTypeId",
                table: "buyMaterialsDocuments",
                column: "DocTypeId",
                principalTable: "BuyDocTypeDefs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_buyMaterialsDocuments_FiscalPeriod_FiscalPeriodId",
                table: "buyMaterialsDocuments",
                column: "FiscalPeriodId",
                principalTable: "FiscalPeriod",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_buyMaterialsDocuments_Section_SectionId",
                table: "buyMaterialsDocuments",
                column: "SectionId",
                principalTable: "Section",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_buyMaterialsDocuments_Transactors_SupplierId",
                table: "buyMaterialsDocuments",
                column: "SupplierId",
                principalTable: "Transactors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTransactions_FiscalPeriod_FiscalPeriodId",
                table: "CustomerTransactions",
                column: "FiscalPeriodId",
                principalTable: "FiscalPeriod",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTransactions_Section_SectionId",
                table: "CustomerTransactions",
                column: "SectionId",
                principalTable: "Section",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierTransactions_FiscalPeriod_FiscalPeriodId",
                table: "SupplierTransactions",
                column: "FiscalPeriodId",
                principalTable: "FiscalPeriod",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierTransactions_Section_SectionId",
                table: "SupplierTransactions",
                column: "SectionId",
                principalTable: "Section",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseTransactions_FiscalPeriod_FiscalPeriodId",
                table: "WarehouseTransactions",
                column: "FiscalPeriodId",
                principalTable: "FiscalPeriod",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseTransactions_Section_SectionId",
                table: "WarehouseTransactions",
                column: "SectionId",
                principalTable: "Section",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
