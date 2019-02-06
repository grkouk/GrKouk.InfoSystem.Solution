﻿using System.ComponentModel;

namespace GrKouk.InfoSystem.Domain.FinConfig
{
    public enum DiaryTransactionsKindEnum
    {
        Expence = 1,
        Income = 2
    }

    /// <summary>
    /// Material Nature
    /// Υλικό, Υπηρεσία, Πάγιο, Δαπάνη
    /// </summary>
    public enum MaterialNatureEnum
    {
        [Description("Χωρίς Προσδιορισμό")]
        MaterialNatureEnumUndefined = 0,
        [Description("Υλικό")]
        MaterialNatureEnumMaterial = 1,
        [Description("Υπηρεσία")]
        MaterialNatureEnumService = 2,
        [Description("Δαπάνη")]
        MaterialNatureEnumExpense = 3,
        [Description("Εσοδο")]
        MaterialNatureEnumIncome = 4,
        [Description("Πάγιο")]
        MaterialNatureEnumFixedAsset = 5
    }
    public enum MaterialTypeEnum
    {
        MaterialTypeNormal = 1,
        MaterialTypeSet = 2,
        MaterialTypeComposed = 3

    }
    /// <summary>
    /// Deprecated
    /// Τύπος κίνησης
    /// 1=Χρεωση
    /// 2=Πίστωση
    /// 
    /// </summary>
    public enum FinancialTransactionTypeEnum
    {
        FinancialTransactionTypeIgnore = 0,
        FinancialTransactionTypeDebit = 1,
        FinancialTransactionTypeCredit = 2
    }
    /// <summary>
    /// Deprecated
    /// Τύπος κίνησης Αποθήκης
    /// 1=Εισαγωγή
    /// 2=Εξαγωγή
    /// 
    /// </summary>
    public enum WarehouseTransactionTypeEnum
    {
        WarehouseTransactionTypeIgnore = 0,
        WarehouseTransactionTypeImport = 1,
        WarehouseTransactionTypeExport = 2
    }
    /// <summary>
    /// Warehouse Transaction New Type Enum
    /// </summary>
    public enum InventoryActionEnum
    {
        InventoryActionEnumNoChange = 0,
        InventoryActionEnumImport = 1,
        InventoryActionEnumExport = 2,
        InventoryActionEnumNegativeImport = 3,
        InventoryActionEnumNegativeExport = 4
    }
    public enum InventoryValueActionEnum
    {
        InventoryValueActionEnumNoChange = 0,
        InventoryValueActionEnumIncrease = 1,
        InventoryValueActionEnumDecrease = 2,
        InventoryValueActionEnumNegativeIncrease = 3,
        InventoryValueActionEnumNegativeDecrease = 4
    }
    public enum FinActionsEnum
    {
        FinActionsEnumNoChange = 0,
        FinActionsEnumDebit = 1,
        FinActionsEnumCredit = 2,
        FinActionsEnumNegativeDebit = 3,
        FinActionsEnumNegativeCredit = 4
    }

    public enum PriceTypeEnum
    {
        PriceTypeEnumNetto = 1,
        PriceTypeEnumBrutto = 2
    }

    public enum MaterialCodeTypeEnum
    {
        CodeTypeEnumCode = 1,
        CodeTypeEnumBarcode = 2,
        CodeTypeEnumSupplierCode = 3
    }

    public enum MaterialCodeUsedUnitEnum
    {
        CodeUsedUnitEnumMain = 1,
        CodeUsedUnitEnumSecondary = 2,
        CodeUsedUnitEnumBuy = 3
    }

    public enum DiaryTypeEnum
    {
        [Description("Πωλήσεις")]
        DiaryTypeEnumSales = 1,
        [Description("Αγορές")]
        DiaryTypeEnumBuys = 2,
        [Description("Δαπάνες")]
        DiaryTypeEnumExpenses = 3,
        [Description("Εσοδα")]
        DiaryTypeEnumIncome = 4,
        [Description("Κινήσεις Αποθήκης")]
        DiaryTypeEnumWarehouse = 5,
        [Description("Κινήσεις Συναλλασσόμενων")]
        DiaryTypeEnumTransactors = 6
    }
}
