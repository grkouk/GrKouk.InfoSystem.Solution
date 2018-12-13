using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class MaterialEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaterialCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 20, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MeasureUnits",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    DecimalPlaces = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasureUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 20, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    ShortDescription = table.Column<string>(maxLength: 500, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    MainMeasureUnitId = table.Column<int>(nullable: true),
                    SecondaryMeasureUnitId = table.Column<int>(nullable: true),
                    SecondaryUnitToMainRate = table.Column<double>(nullable: false),
                    BuyMeasureUnitId = table.Column<int>(nullable: true),
                    BuyUnitToMainRate = table.Column<double>(nullable: false),
                    FpaDefId = table.Column<int>(nullable: false),
                    BarCode = table.Column<string>(maxLength: 50, nullable: true),
                    ManufacturerCode = table.Column<string>(maxLength: 50, nullable: true),
                    MaterialCategoryId = table.Column<int>(nullable: false),
                    MaterialType = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_MeasureUnits_Code",
                table: "MeasureUnits",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "MeasureUnits");

            migrationBuilder.DropTable(
                name: "MaterialCategories");
        }
    }
}
