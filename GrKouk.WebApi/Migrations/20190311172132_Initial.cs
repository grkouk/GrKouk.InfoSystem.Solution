using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrKouk.WebApi.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CostCentres",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostCentres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiaryDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    DiaryType = table.Column<int>(nullable: false),
                    SelectedDocTypes = table.Column<string>(maxLength: 200, nullable: true),
                    SelectedMatNatures = table.Column<string>(maxLength: 200, nullable: true),
                    SelectedTransTypes = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaryDefs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinancialMovements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Action = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialMovements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinTransCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinTransCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FiscalPeriods",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FiscalPeriods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FpaKategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Rate = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FpaKategories", x => x.Id);
                });

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
                name: "RevenueCentres",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevenueCentres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 25, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    SystemName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactorTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsSystem = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(maxLength: 15, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactorTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransExpenseDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 20, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    InventoryAction = table.Column<int>(nullable: false),
                    InventoryValueAction = table.Column<int>(nullable: false),
                    DefaultDocSeriesId = table.Column<int>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransExpenseDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransExpenseDefs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransWarehouseDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    MaterialInventoryAction = table.Column<int>(nullable: false),
                    MaterialInventoryValueAction = table.Column<int>(nullable: false),
                    MaterialInvoicedVolumeAction = table.Column<int>(nullable: false),
                    MaterialInvoicedValueAction = table.Column<int>(nullable: false),
                    ServiceInventoryAction = table.Column<int>(nullable: false),
                    ServiceInventoryValueAction = table.Column<int>(nullable: false),
                    ExpenseInventoryAction = table.Column<int>(nullable: false),
                    ExpenseInventoryValueAction = table.Column<int>(nullable: false),
                    IncomeInventoryAction = table.Column<int>(nullable: false),
                    IncomeInventoryValueAction = table.Column<int>(nullable: false),
                    FixedAssetInventoryAction = table.Column<int>(nullable: false),
                    FixedAssetInventoryValueAction = table.Column<int>(nullable: false),
                    DefaultDocSeriesId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransWarehouseDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransWarehouseDefs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransTransactorDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    FinancialTransAction = table.Column<int>(nullable: false),
                    TurnOverTransId = table.Column<int>(nullable: false),
                    DefaultDocSeriesId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransTransactorDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransTransactorDefs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransTransactorDefs_FinancialMovements_TurnOverTransId",
                        column: x => x.TurnOverTransId,
                        principalTable: "FinancialMovements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "Transactors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Address = table.Column<string>(maxLength: 200, nullable: true),
                    City = table.Column<string>(maxLength: 50, nullable: true),
                    Zip = table.Column<int>(nullable: true),
                    PhoneWork = table.Column<string>(maxLength: 200, nullable: true),
                    PhoneMobile = table.Column<string>(maxLength: 200, nullable: true),
                    PhoneFax = table.Column<string>(maxLength: 200, nullable: true),
                    EMail = table.Column<string>(maxLength: 200, nullable: true),
                    TransactorTypeId = table.Column<int>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactors_TransactorTypes_TransactorTypeId",
                        column: x => x.TransactorTypeId,
                        principalTable: "TransactorTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransWarehouseDocTypeDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    TransWarehouseDefId = table.Column<int>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransWarehouseDocTypeDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransWarehouseDocTypeDefs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransWarehouseDocTypeDefs_TransWarehouseDefs_TransWarehouseDefId",
                        column: x => x.TransWarehouseDefId,
                        principalTable: "TransWarehouseDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BuyDocTypeDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: false),
                    Abbr = table.Column<string>(maxLength: 20, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    TransTransactorDefId = table.Column<int>(nullable: true),
                    TransWarehouseDefId = table.Column<int>(nullable: true),
                    UsedPrice = table.Column<int>(nullable: false),
                    SelectedWarehouseItemNatures = table.Column<string>(maxLength: 200, nullable: true),
                    AllowedTransactorTypes = table.Column<string>(maxLength: 200, nullable: true),
                    AllowedSectionTypes = table.Column<string>(maxLength: 200, nullable: true),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyDocTypeDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuyDocTypeDefs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuyDocTypeDefs_TransTransactorDefs_TransTransactorDefId",
                        column: x => x.TransTransactorDefId,
                        principalTable: "TransTransactorDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuyDocTypeDefs_TransWarehouseDefs_TransWarehouseDefId",
                        column: x => x.TransWarehouseDefId,
                        principalTable: "TransWarehouseDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SellDocTypeDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: false),
                    Abbr = table.Column<string>(maxLength: 20, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    TransTransactorDefId = table.Column<int>(nullable: true),
                    TransWarehouseDefId = table.Column<int>(nullable: true),
                    UsedPrice = table.Column<int>(nullable: false),
                    SelectedWarehouseItemNatures = table.Column<string>(maxLength: 200, nullable: true),
                    AllowedTransactorTypes = table.Column<string>(maxLength: 200, nullable: true),
                    AllowedSectionTypes = table.Column<string>(maxLength: 200, nullable: true),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellDocTypeDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellDocTypeDefs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellDocTypeDefs_TransTransactorDefs_TransTransactorDefId",
                        column: x => x.TransTransactorDefId,
                        principalTable: "TransTransactorDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellDocTypeDefs_TransWarehouseDefs_TransWarehouseDefId",
                        column: x => x.TransWarehouseDefId,
                        principalTable: "TransWarehouseDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransTransactorDocTypeDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    TransTransactorDefId = table.Column<int>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransTransactorDocTypeDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransTransactorDocTypeDefs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransTransactorDocTypeDefs_TransTransactorDefs_TransTransactorDefId",
                        column: x => x.TransTransactorDefId,
                        principalTable: "TransTransactorDefs",
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

            migrationBuilder.CreateTable(
                name: "FinDiaryTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    ReferenceCode = table.Column<string>(maxLength: 50, nullable: true),
                    TransactorId = table.Column<int>(nullable: false),
                    FinTransCategoryId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    CostCentreId = table.Column<int>(nullable: false),
                    RevenueCentreId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Kind = table.Column<int>(nullable: false),
                    AmountFpa = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountNet = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinDiaryTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinDiaryTransactions_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FinDiaryTransactions_CostCentres_CostCentreId",
                        column: x => x.CostCentreId,
                        principalTable: "CostCentres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FinDiaryTransactions_FinTransCategories_FinTransCategoryId",
                        column: x => x.FinTransCategoryId,
                        principalTable: "FinTransCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FinDiaryTransactions_RevenueCentres_RevenueCentreId",
                        column: x => x.RevenueCentreId,
                        principalTable: "RevenueCentres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FinDiaryTransactions_Transactors_TransactorId",
                        column: x => x.TransactorId,
                        principalTable: "Transactors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransWarehouseDocSeriesDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    TransWarehouseDocTypeDefId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransWarehouseDocSeriesDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransWarehouseDocSeriesDefs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransWarehouseDocSeriesDefs_TransWarehouseDocTypeDefs_TransWarehouseDocTypeDefId",
                        column: x => x.TransWarehouseDocTypeDefId,
                        principalTable: "TransWarehouseDocTypeDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BuyDocSeriesDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: false),
                    Abbr = table.Column<string>(maxLength: 20, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    BuyDocTypeDefId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyDocSeriesDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuyDocSeriesDefs_BuyDocTypeDefs_BuyDocTypeDefId",
                        column: x => x.BuyDocTypeDefId,
                        principalTable: "BuyDocTypeDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuyDocSeriesDefs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SellDocSeriesDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: false),
                    Abbr = table.Column<string>(maxLength: 20, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    SellDocTypeDefId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellDocSeriesDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellDocSeriesDefs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellDocSeriesDefs_SellDocTypeDefs_SellDocTypeDefId",
                        column: x => x.SellDocTypeDefId,
                        principalTable: "SellDocTypeDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransTransactorDocSeriesDefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    TransTransactorDocTypeDefId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransTransactorDocSeriesDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransTransactorDocSeriesDefs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransTransactorDocSeriesDefs_TransTransactorDocTypeDefs_TransTransactorDocTypeDefId",
                        column: x => x.TransTransactorDocTypeDefId,
                        principalTable: "TransTransactorDocTypeDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransDate = table.Column<DateTime>(nullable: false),
                    TransWarehouseDocSeriesId = table.Column<int>(nullable: false),
                    TransWarehouseDocTypeId = table.Column<int>(nullable: false),
                    TransRefCode = table.Column<string>(nullable: true),
                    SectionId = table.Column<int>(nullable: false),
                    WarehouseItemId = table.Column<int>(nullable: false),
                    FiscalPeriodId = table.Column<int>(nullable: false),
                    TransactionType = table.Column<int>(nullable: false),
                    InventoryAction = table.Column<int>(nullable: false),
                    InventoryValueAction = table.Column<int>(nullable: false),
                    InvoicedVolumeAction = table.Column<int>(nullable: false),
                    InvoicedValueAction = table.Column<int>(nullable: false),
                    CreatorId = table.Column<int>(nullable: false),
                    PrimaryUnitId = table.Column<int>(nullable: false),
                    SecondaryUnitId = table.Column<int>(nullable: false),
                    UnitFactor = table.Column<decimal>(nullable: false),
                    Quontity1 = table.Column<double>(nullable: false),
                    Quontity2 = table.Column<double>(nullable: false),
                    FpaRate = table.Column<decimal>(nullable: false),
                    DiscountRate = table.Column<decimal>(nullable: false),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    UnitExpenses = table.Column<decimal>(nullable: false),
                    AmountFpa = table.Column<decimal>(nullable: false),
                    AmountNet = table.Column<decimal>(nullable: false),
                    AmountDiscount = table.Column<decimal>(nullable: false),
                    TransQ1 = table.Column<decimal>(nullable: false),
                    TransQ2 = table.Column<decimal>(nullable: false),
                    TransFpaAmount = table.Column<decimal>(nullable: false),
                    TransNetAmount = table.Column<decimal>(nullable: false),
                    TransDiscountAmount = table.Column<decimal>(nullable: false),
                    Etiology = table.Column<string>(maxLength: 500, nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseTransactions_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseTransactions_FiscalPeriods_FiscalPeriodId",
                        column: x => x.FiscalPeriodId,
                        principalTable: "FiscalPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseTransactions_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseTransactions_TransWarehouseDocSeriesDefs_TransWarehouseDocSeriesId",
                        column: x => x.TransWarehouseDocSeriesId,
                        principalTable: "TransWarehouseDocSeriesDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseTransactions_TransWarehouseDocTypeDefs_TransWarehouseDocTypeId",
                        column: x => x.TransWarehouseDocTypeId,
                        principalTable: "TransWarehouseDocTypeDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseTransactions_WarehouseItems_WarehouseItemId",
                        column: x => x.WarehouseItemId,
                        principalTable: "WarehouseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BuyDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransDate = table.Column<DateTime>(nullable: false),
                    TransRefCode = table.Column<string>(nullable: true),
                    SectionId = table.Column<int>(nullable: false),
                    TransactorId = table.Column<int>(nullable: false),
                    FiscalPeriodId = table.Column<int>(nullable: false),
                    BuyDocSeriesId = table.Column<int>(nullable: false),
                    BuyDocTypeId = table.Column<int>(nullable: false),
                    AmountFpa = table.Column<decimal>(nullable: false),
                    AmountNet = table.Column<decimal>(nullable: false),
                    AmountDiscount = table.Column<decimal>(nullable: false),
                    Etiology = table.Column<string>(maxLength: 500, nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuyDocuments_BuyDocSeriesDefs_BuyDocSeriesId",
                        column: x => x.BuyDocSeriesId,
                        principalTable: "BuyDocSeriesDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BuyDocuments_BuyDocTypeDefs_BuyDocTypeId",
                        column: x => x.BuyDocTypeId,
                        principalTable: "BuyDocTypeDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BuyDocuments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuyDocuments_FiscalPeriods_FiscalPeriodId",
                        column: x => x.FiscalPeriodId,
                        principalTable: "FiscalPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuyDocuments_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuyDocuments_Transactors_TransactorId",
                        column: x => x.TransactorId,
                        principalTable: "Transactors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SellDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransDate = table.Column<DateTime>(nullable: false),
                    TransRefCode = table.Column<string>(nullable: true),
                    SectionId = table.Column<int>(nullable: false),
                    TransactorId = table.Column<int>(nullable: false),
                    FiscalPeriodId = table.Column<int>(nullable: false),
                    SellDocSeriesId = table.Column<int>(nullable: false),
                    SellDocTypeId = table.Column<int>(nullable: false),
                    AmountFpa = table.Column<decimal>(nullable: false),
                    AmountNet = table.Column<decimal>(nullable: false),
                    AmountDiscount = table.Column<decimal>(nullable: false),
                    Etiology = table.Column<string>(maxLength: 500, nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellDocuments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellDocuments_FiscalPeriods_FiscalPeriodId",
                        column: x => x.FiscalPeriodId,
                        principalTable: "FiscalPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellDocuments_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellDocuments_SellDocSeriesDefs_SellDocSeriesId",
                        column: x => x.SellDocSeriesId,
                        principalTable: "SellDocSeriesDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SellDocuments_SellDocTypeDefs_SellDocTypeId",
                        column: x => x.SellDocTypeId,
                        principalTable: "SellDocTypeDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SellDocuments_Transactors_TransactorId",
                        column: x => x.TransactorId,
                        principalTable: "Transactors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransactorTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransDate = table.Column<DateTime>(nullable: false),
                    TransTransactorDocSeriesId = table.Column<int>(nullable: false),
                    TransTransactorDocTypeId = table.Column<int>(nullable: false),
                    TransRefCode = table.Column<string>(nullable: true),
                    TransactorId = table.Column<int>(nullable: false),
                    SectionId = table.Column<int>(nullable: false),
                    CreatorId = table.Column<int>(nullable: false),
                    FiscalPeriodId = table.Column<int>(nullable: false),
                    FinancialAction = table.Column<int>(nullable: false),
                    FpaRate = table.Column<decimal>(nullable: false),
                    DiscountRate = table.Column<decimal>(nullable: false),
                    AmountFpa = table.Column<decimal>(nullable: false),
                    AmountNet = table.Column<decimal>(nullable: false),
                    AmountDiscount = table.Column<decimal>(nullable: false),
                    TransFpaAmount = table.Column<decimal>(nullable: false),
                    TransNetAmount = table.Column<decimal>(nullable: false),
                    TransDiscountAmount = table.Column<decimal>(nullable: false),
                    Etiology = table.Column<string>(maxLength: 500, nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactorTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactorTransactions_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactorTransactions_FiscalPeriods_FiscalPeriodId",
                        column: x => x.FiscalPeriodId,
                        principalTable: "FiscalPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactorTransactions_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactorTransactions_TransTransactorDocSeriesDefs_TransTransactorDocSeriesId",
                        column: x => x.TransTransactorDocSeriesId,
                        principalTable: "TransTransactorDocSeriesDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactorTransactions_TransTransactorDocTypeDefs_TransTransactorDocTypeId",
                        column: x => x.TransTransactorDocTypeId,
                        principalTable: "TransTransactorDocTypeDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactorTransactions_Transactors_TransactorId",
                        column: x => x.TransactorId,
                        principalTable: "Transactors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BuyDocLines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BuyDocumentId = table.Column<int>(nullable: false),
                    WarehouseItemId = table.Column<int>(nullable: false),
                    PrimaryUnitId = table.Column<int>(nullable: false),
                    SecondaryUnitId = table.Column<int>(nullable: false),
                    Factor = table.Column<float>(nullable: false),
                    Quontity1 = table.Column<double>(nullable: false),
                    Quontity2 = table.Column<double>(nullable: false),
                    FpaRate = table.Column<decimal>(nullable: false),
                    DiscountRate = table.Column<decimal>(nullable: false),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    AmountFpa = table.Column<decimal>(nullable: false),
                    AmountNet = table.Column<decimal>(nullable: false),
                    AmountDiscount = table.Column<decimal>(nullable: false),
                    Etiology = table.Column<string>(maxLength: 500, nullable: true),
                    BuyDocumentId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyDocLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuyDocLines_BuyDocuments_BuyDocumentId",
                        column: x => x.BuyDocumentId,
                        principalTable: "BuyDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuyDocLines_BuyDocuments_BuyDocumentId1",
                        column: x => x.BuyDocumentId1,
                        principalTable: "BuyDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuyDocLines_WarehouseItems_WarehouseItemId",
                        column: x => x.WarehouseItemId,
                        principalTable: "WarehouseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SellDocLines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SellDocumentId = table.Column<int>(nullable: false),
                    WarehouseItemId = table.Column<int>(nullable: false),
                    PrimaryUnitId = table.Column<int>(nullable: false),
                    SecondaryUnitId = table.Column<int>(nullable: false),
                    Factor = table.Column<float>(nullable: false),
                    Quontity1 = table.Column<double>(nullable: false),
                    Quontity2 = table.Column<double>(nullable: false),
                    FpaRate = table.Column<decimal>(nullable: false),
                    DiscountRate = table.Column<decimal>(nullable: false),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    AmountFpa = table.Column<decimal>(nullable: false),
                    AmountNet = table.Column<decimal>(nullable: false),
                    AmountDiscount = table.Column<decimal>(nullable: false),
                    Etiology = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellDocLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellDocLines_SellDocuments_SellDocumentId",
                        column: x => x.SellDocumentId,
                        principalTable: "SellDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellDocLines_WarehouseItems_WarehouseItemId",
                        column: x => x.WarehouseItemId,
                        principalTable: "WarehouseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocLines_BuyDocumentId",
                table: "BuyDocLines",
                column: "BuyDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocLines_BuyDocumentId1",
                table: "BuyDocLines",
                column: "BuyDocumentId1");

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocLines_WarehouseItemId",
                table: "BuyDocLines",
                column: "WarehouseItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocSeriesDefs_BuyDocTypeDefId",
                table: "BuyDocSeriesDefs",
                column: "BuyDocTypeDefId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocSeriesDefs_Code",
                table: "BuyDocSeriesDefs",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocSeriesDefs_CompanyId",
                table: "BuyDocSeriesDefs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocTypeDefs_Code",
                table: "BuyDocTypeDefs",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocTypeDefs_CompanyId",
                table: "BuyDocTypeDefs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocTypeDefs_TransTransactorDefId",
                table: "BuyDocTypeDefs",
                column: "TransTransactorDefId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocTypeDefs_TransWarehouseDefId",
                table: "BuyDocTypeDefs",
                column: "TransWarehouseDefId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocuments_BuyDocSeriesId",
                table: "BuyDocuments",
                column: "BuyDocSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocuments_BuyDocTypeId",
                table: "BuyDocuments",
                column: "BuyDocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocuments_CompanyId",
                table: "BuyDocuments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocuments_FiscalPeriodId",
                table: "BuyDocuments",
                column: "FiscalPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocuments_SectionId",
                table: "BuyDocuments",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocuments_TransDate",
                table: "BuyDocuments",
                column: "TransDate");

            migrationBuilder.CreateIndex(
                name: "IX_BuyDocuments_TransactorId",
                table: "BuyDocuments",
                column: "TransactorId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialMovements_Code",
                table: "FinancialMovements",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FinDiaryTransactions_CompanyId",
                table: "FinDiaryTransactions",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_FinDiaryTransactions_CostCentreId",
                table: "FinDiaryTransactions",
                column: "CostCentreId");

            migrationBuilder.CreateIndex(
                name: "IX_FinDiaryTransactions_FinTransCategoryId",
                table: "FinDiaryTransactions",
                column: "FinTransCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FinDiaryTransactions_RevenueCentreId",
                table: "FinDiaryTransactions",
                column: "RevenueCentreId");

            migrationBuilder.CreateIndex(
                name: "IX_FinDiaryTransactions_TransactionDate",
                table: "FinDiaryTransactions",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_FinDiaryTransactions_TransactorId",
                table: "FinDiaryTransactions",
                column: "TransactorId");

            migrationBuilder.CreateIndex(
                name: "IX_FinTransCategories_Code",
                table: "FinTransCategories",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_FiscalPeriods_Code",
                table: "FiscalPeriods",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FpaKategories_Code",
                table: "FpaKategories",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MeasureUnits_Code",
                table: "MeasureUnits",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sections_Code",
                table: "Sections",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SellDocLines_SellDocumentId",
                table: "SellDocLines",
                column: "SellDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_SellDocLines_WarehouseItemId",
                table: "SellDocLines",
                column: "WarehouseItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SellDocSeriesDefs_Code",
                table: "SellDocSeriesDefs",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SellDocSeriesDefs_CompanyId",
                table: "SellDocSeriesDefs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SellDocSeriesDefs_SellDocTypeDefId",
                table: "SellDocSeriesDefs",
                column: "SellDocTypeDefId");

            migrationBuilder.CreateIndex(
                name: "IX_SellDocTypeDefs_Code",
                table: "SellDocTypeDefs",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SellDocTypeDefs_CompanyId",
                table: "SellDocTypeDefs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SellDocTypeDefs_TransTransactorDefId",
                table: "SellDocTypeDefs",
                column: "TransTransactorDefId");

            migrationBuilder.CreateIndex(
                name: "IX_SellDocTypeDefs_TransWarehouseDefId",
                table: "SellDocTypeDefs",
                column: "TransWarehouseDefId");

            migrationBuilder.CreateIndex(
                name: "IX_SellDocuments_CompanyId",
                table: "SellDocuments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SellDocuments_FiscalPeriodId",
                table: "SellDocuments",
                column: "FiscalPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_SellDocuments_SectionId",
                table: "SellDocuments",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_SellDocuments_SellDocSeriesId",
                table: "SellDocuments",
                column: "SellDocSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_SellDocuments_SellDocTypeId",
                table: "SellDocuments",
                column: "SellDocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SellDocuments_TransDate",
                table: "SellDocuments",
                column: "TransDate");

            migrationBuilder.CreateIndex(
                name: "IX_SellDocuments_TransactorId",
                table: "SellDocuments",
                column: "TransactorId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactors_Code",
                table: "Transactors",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Transactors_TransactorTypeId",
                table: "Transactors",
                column: "TransactorTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactorTransactions_CompanyId",
                table: "TransactorTransactions",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactorTransactions_CreatorId",
                table: "TransactorTransactions",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactorTransactions_FiscalPeriodId",
                table: "TransactorTransactions",
                column: "FiscalPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactorTransactions_SectionId",
                table: "TransactorTransactions",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactorTransactions_TransDate",
                table: "TransactorTransactions",
                column: "TransDate");

            migrationBuilder.CreateIndex(
                name: "IX_TransactorTransactions_TransTransactorDocSeriesId",
                table: "TransactorTransactions",
                column: "TransTransactorDocSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactorTransactions_TransTransactorDocTypeId",
                table: "TransactorTransactions",
                column: "TransTransactorDocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactorTransactions_TransactorId",
                table: "TransactorTransactions",
                column: "TransactorId");

            migrationBuilder.CreateIndex(
                name: "IX_TransExpenseDefs_Code",
                table: "TransExpenseDefs",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TransExpenseDefs_CompanyId",
                table: "TransExpenseDefs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TransTransactorDefs_Code",
                table: "TransTransactorDefs",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TransTransactorDefs_CompanyId",
                table: "TransTransactorDefs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TransTransactorDefs_TurnOverTransId",
                table: "TransTransactorDefs",
                column: "TurnOverTransId");

            migrationBuilder.CreateIndex(
                name: "IX_TransTransactorDocSeriesDefs_Code",
                table: "TransTransactorDocSeriesDefs",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TransTransactorDocSeriesDefs_CompanyId",
                table: "TransTransactorDocSeriesDefs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TransTransactorDocSeriesDefs_TransTransactorDocTypeDefId",
                table: "TransTransactorDocSeriesDefs",
                column: "TransTransactorDocTypeDefId");

            migrationBuilder.CreateIndex(
                name: "IX_TransTransactorDocTypeDefs_Code",
                table: "TransTransactorDocTypeDefs",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TransTransactorDocTypeDefs_CompanyId",
                table: "TransTransactorDocTypeDefs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TransTransactorDocTypeDefs_TransTransactorDefId",
                table: "TransTransactorDocTypeDefs",
                column: "TransTransactorDefId");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDefs_Code",
                table: "TransWarehouseDefs",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDefs_CompanyId",
                table: "TransWarehouseDefs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDocSeriesDefs_Code",
                table: "TransWarehouseDocSeriesDefs",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDocSeriesDefs_CompanyId",
                table: "TransWarehouseDocSeriesDefs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDocSeriesDefs_TransWarehouseDocTypeDefId",
                table: "TransWarehouseDocSeriesDefs",
                column: "TransWarehouseDocTypeDefId");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDocTypeDefs_Code",
                table: "TransWarehouseDocTypeDefs",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDocTypeDefs_CompanyId",
                table: "TransWarehouseDocTypeDefs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TransWarehouseDocTypeDefs_TransWarehouseDefId",
                table: "TransWarehouseDocTypeDefs",
                column: "TransWarehouseDefId");

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

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTransactions_CompanyId",
                table: "WarehouseTransactions",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTransactions_CreatorId",
                table: "WarehouseTransactions",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTransactions_FiscalPeriodId",
                table: "WarehouseTransactions",
                column: "FiscalPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTransactions_SectionId",
                table: "WarehouseTransactions",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTransactions_TransDate",
                table: "WarehouseTransactions",
                column: "TransDate");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTransactions_TransWarehouseDocSeriesId",
                table: "WarehouseTransactions",
                column: "TransWarehouseDocSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTransactions_TransWarehouseDocTypeId",
                table: "WarehouseTransactions",
                column: "TransWarehouseDocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTransactions_WarehouseItemId",
                table: "WarehouseTransactions",
                column: "WarehouseItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuyDocLines");

            migrationBuilder.DropTable(
                name: "DiaryDefs");

            migrationBuilder.DropTable(
                name: "FinDiaryTransactions");

            migrationBuilder.DropTable(
                name: "SellDocLines");

            migrationBuilder.DropTable(
                name: "TransactorTransactions");

            migrationBuilder.DropTable(
                name: "TransExpenseDefs");

            migrationBuilder.DropTable(
                name: "WarehouseItemsCodes");

            migrationBuilder.DropTable(
                name: "WarehouseTransactions");

            migrationBuilder.DropTable(
                name: "BuyDocuments");

            migrationBuilder.DropTable(
                name: "CostCentres");

            migrationBuilder.DropTable(
                name: "FinTransCategories");

            migrationBuilder.DropTable(
                name: "RevenueCentres");

            migrationBuilder.DropTable(
                name: "SellDocuments");

            migrationBuilder.DropTable(
                name: "TransTransactorDocSeriesDefs");

            migrationBuilder.DropTable(
                name: "TransWarehouseDocSeriesDefs");

            migrationBuilder.DropTable(
                name: "WarehouseItems");

            migrationBuilder.DropTable(
                name: "BuyDocSeriesDefs");

            migrationBuilder.DropTable(
                name: "FiscalPeriods");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "SellDocSeriesDefs");

            migrationBuilder.DropTable(
                name: "Transactors");

            migrationBuilder.DropTable(
                name: "TransTransactorDocTypeDefs");

            migrationBuilder.DropTable(
                name: "TransWarehouseDocTypeDefs");

            migrationBuilder.DropTable(
                name: "MeasureUnits");

            migrationBuilder.DropTable(
                name: "FpaKategories");

            migrationBuilder.DropTable(
                name: "MaterialCategories");

            migrationBuilder.DropTable(
                name: "BuyDocTypeDefs");

            migrationBuilder.DropTable(
                name: "SellDocTypeDefs");

            migrationBuilder.DropTable(
                name: "TransactorTypes");

            migrationBuilder.DropTable(
                name: "TransTransactorDefs");

            migrationBuilder.DropTable(
                name: "TransWarehouseDefs");

            migrationBuilder.DropTable(
                name: "FinancialMovements");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
