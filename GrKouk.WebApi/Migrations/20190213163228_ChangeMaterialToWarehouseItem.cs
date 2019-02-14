using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class ChangeMaterialToWarehouseItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuyDocLines_Materials_MaterialId",
                table: "BuyDocLines");

            migrationBuilder.DropForeignKey(
                name: "FK_SellDocLines_Materials_MaterialId",
                table: "SellDocLines");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseTransactions_Materials_MaterialId",
                table: "WarehouseTransactions");

            migrationBuilder.DropTable(
                name: "MaterialCodes");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.RenameColumn(
                name: "MaterialId",
                table: "WarehouseTransactions",
                newName: "WarehouseItemId");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseTransactions_MaterialId",
                table: "WarehouseTransactions",
                newName: "IX_WarehouseTransactions_WarehouseItemId");

            migrationBuilder.RenameColumn(
                name: "MaterialId",
                table: "SellDocLines",
                newName: "WarehouseItemId");

            migrationBuilder.RenameIndex(
                name: "IX_SellDocLines_MaterialId",
                table: "SellDocLines",
                newName: "IX_SellDocLines_WarehouseItemId");

            migrationBuilder.RenameColumn(
                name: "MaterialId",
                table: "BuyDocLines",
                newName: "WarehouseItemId");

            migrationBuilder.RenameIndex(
                name: "IX_BuyDocLines_MaterialId",
                table: "BuyDocLines",
                newName: "IX_BuyDocLines_WarehouseItemId");

            migrationBuilder.CreateTable(
                name: "WarehouseItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 20, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    ShortDescription = table.Column<string>(maxLength: 500, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    MainMeasureUnitId = table.Column<int>(nullable: false),
                    SecondaryMeasureUnitId = table.Column<int>(nullable: false),
                    SecondaryUnitToMainRate = table.Column<double>(nullable: false),
                    BuyMeasureUnitId = table.Column<int>(nullable: false),
                    BuyUnitToMainRate = table.Column<double>(nullable: false),
                    FpaDefId = table.Column<int>(nullable: false),
                    BarCode = table.Column<string>(maxLength: 50, nullable: true),
                    ManufacturerCode = table.Column<string>(maxLength: 50, nullable: true),
                    MaterialCategoryId = table.Column<int>(nullable: false),
                    MaterialType = table.Column<int>(nullable: false),
                    WarehouseItemNature = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    PriceNetto = table.Column<decimal>(nullable: false),
                    PriceBrutto = table.Column<decimal>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseItems_MeasureUnits_BuyMeasureUnitId",
                        column: x => x.BuyMeasureUnitId,
                        principalTable: "MeasureUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseItems_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseItems_FpaKategories_FpaDefId",
                        column: x => x.FpaDefId,
                        principalTable: "FpaKategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseItems_MeasureUnits_MainMeasureUnitId",
                        column: x => x.MainMeasureUnitId,
                        principalTable: "MeasureUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseItems_MaterialCategories_MaterialCategoryId",
                        column: x => x.MaterialCategoryId,
                        principalTable: "MaterialCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseItems_MeasureUnits_SecondaryMeasureUnitId",
                        column: x => x.SecondaryMeasureUnitId,
                        principalTable: "MeasureUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseItemsCodes",
                columns: table => new
                {
                    CodeType = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 30, nullable: false),
                    TransactorId = table.Column<int>(nullable: false),
                    WarehouseItemId = table.Column<int>(nullable: false),
                    CodeUsedUnit = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseItemsCodes", x => new { x.CodeType, x.WarehouseItemId, x.Code });
                    table.ForeignKey(
                        name: "FK_WarehouseItemsCodes_WarehouseItems_WarehouseItemId",
                        column: x => x.WarehouseItemId,
                        principalTable: "WarehouseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseItems_BuyMeasureUnitId",
                table: "WarehouseItems",
                column: "BuyMeasureUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseItems_Code",
                table: "WarehouseItems",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseItems_CompanyId",
                table: "WarehouseItems",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseItems_FpaDefId",
                table: "WarehouseItems",
                column: "FpaDefId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseItems_MainMeasureUnitId",
                table: "WarehouseItems",
                column: "MainMeasureUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseItems_MaterialCategoryId",
                table: "WarehouseItems",
                column: "MaterialCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseItems_SecondaryMeasureUnitId",
                table: "WarehouseItems",
                column: "SecondaryMeasureUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseItemsCodes_Code",
                table: "WarehouseItemsCodes",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseItemsCodes_WarehouseItemId",
                table: "WarehouseItemsCodes",
                column: "WarehouseItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_BuyDocLines_WarehouseItems_WarehouseItemId",
                table: "BuyDocLines",
                column: "WarehouseItemId",
                principalTable: "WarehouseItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SellDocLines_WarehouseItems_WarehouseItemId",
                table: "SellDocLines",
                column: "WarehouseItemId",
                principalTable: "WarehouseItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseTransactions_WarehouseItems_WarehouseItemId",
                table: "WarehouseTransactions",
                column: "WarehouseItemId",
                principalTable: "WarehouseItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuyDocLines_WarehouseItems_WarehouseItemId",
                table: "BuyDocLines");

            migrationBuilder.DropForeignKey(
                name: "FK_SellDocLines_WarehouseItems_WarehouseItemId",
                table: "SellDocLines");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseTransactions_WarehouseItems_WarehouseItemId",
                table: "WarehouseTransactions");

            migrationBuilder.DropTable(
                name: "WarehouseItemsCodes");

            migrationBuilder.DropTable(
                name: "WarehouseItems");

            migrationBuilder.RenameColumn(
                name: "WarehouseItemId",
                table: "WarehouseTransactions",
                newName: "MaterialId");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseTransactions_WarehouseItemId",
                table: "WarehouseTransactions",
                newName: "IX_WarehouseTransactions_MaterialId");

            migrationBuilder.RenameColumn(
                name: "WarehouseItemId",
                table: "SellDocLines",
                newName: "MaterialId");

            migrationBuilder.RenameIndex(
                name: "IX_SellDocLines_WarehouseItemId",
                table: "SellDocLines",
                newName: "IX_SellDocLines_MaterialId");

            migrationBuilder.RenameColumn(
                name: "WarehouseItemId",
                table: "BuyDocLines",
                newName: "MaterialId");

            migrationBuilder.RenameIndex(
                name: "IX_BuyDocLines_WarehouseItemId",
                table: "BuyDocLines",
                newName: "IX_BuyDocLines_MaterialId");

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    BarCode = table.Column<string>(maxLength: 50, nullable: true),
                    BuyMeasureUnitId = table.Column<int>(nullable: false),
                    BuyUnitToMainRate = table.Column<double>(nullable: false),
                    Code = table.Column<string>(maxLength: 20, nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    FpaDefId = table.Column<int>(nullable: false),
                    MainMeasureUnitId = table.Column<int>(nullable: false),
                    ManufacturerCode = table.Column<string>(maxLength: 50, nullable: true),
                    MaterialCategoryId = table.Column<int>(nullable: false),
                    MaterialNature = table.Column<int>(nullable: false),
                    MaterialType = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    PriceBrutto = table.Column<decimal>(nullable: false),
                    PriceNetto = table.Column<decimal>(nullable: false),
                    SecondaryMeasureUnitId = table.Column<int>(nullable: false),
                    SecondaryUnitToMainRate = table.Column<double>(nullable: false),
                    ShortDescription = table.Column<string>(maxLength: 500, nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Materials_MeasureUnits_BuyMeasureUnitId",
                        column: x => x.BuyMeasureUnitId,
                        principalTable: "MeasureUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Materials_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Materials_FpaKategories_FpaDefId",
                        column: x => x.FpaDefId,
                        principalTable: "FpaKategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Materials_MeasureUnits_MainMeasureUnitId",
                        column: x => x.MainMeasureUnitId,
                        principalTable: "MeasureUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Materials_MaterialCategories_MaterialCategoryId",
                        column: x => x.MaterialCategoryId,
                        principalTable: "MaterialCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Materials_MeasureUnits_SecondaryMeasureUnitId",
                        column: x => x.SecondaryMeasureUnitId,
                        principalTable: "MeasureUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaterialCodes",
                columns: table => new
                {
                    CodeType = table.Column<int>(nullable: false),
                    MaterialId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 30, nullable: false),
                    CodeUsedUnit = table.Column<int>(nullable: false),
                    TransactorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialCodes", x => new { x.CodeType, x.MaterialId, x.Code });
                    table.ForeignKey(
                        name: "FK_MaterialCodes_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialCodes_Code",
                table: "MaterialCodes",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialCodes_MaterialId",
                table: "MaterialCodes",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_BuyMeasureUnitId",
                table: "Materials",
                column: "BuyMeasureUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_Code",
                table: "Materials",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_CompanyId",
                table: "Materials",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_FpaDefId",
                table: "Materials",
                column: "FpaDefId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_MainMeasureUnitId",
                table: "Materials",
                column: "MainMeasureUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_MaterialCategoryId",
                table: "Materials",
                column: "MaterialCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_SecondaryMeasureUnitId",
                table: "Materials",
                column: "SecondaryMeasureUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_BuyDocLines_Materials_MaterialId",
                table: "BuyDocLines",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SellDocLines_Materials_MaterialId",
                table: "SellDocLines",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseTransactions_Materials_MaterialId",
                table: "WarehouseTransactions",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
