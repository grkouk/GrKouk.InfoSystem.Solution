using System;
using System.ComponentModel;

namespace GrKouk.InfoSystem.Definitions
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
    public enum WarehouseItemNatureEnum
    {
        [Description("Χωρίς Προσδιορισμό")]
        WarehouseItemNatureUndefined = 0,
        [Description("Υλικό")]
        WarehouseItemNatureMaterial = 1,
        [Description("Υπηρεσία")]
        WarehouseItemNatureService = 2,
        [Description("Δαπάνη")]
        WarehouseItemNatureExpense = 3,
        [Description("Εσοδο")]
        WarehouseItemNatureIncome = 4,
        [Description("Πάγιο")]
        WarehouseItemNatureFixedAsset = 5,
        [Description("Πρώτη Υλη")]
        WarehouseItemNatureRawMaterial = 6
    }
    public enum MaterialTypeEnum
    {
        [Description("Κανονικο")]
        MaterialTypeNormal = 1,
        [Description("Σετ")]
        MaterialTypeSet = 2,
        [Description("Συντιθέμενο")]
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
        [Description("Καμία Μεταβολή")]
        InventoryActionEnumNoChange = 0,
        [Description("Εισαγωγή")]
        InventoryActionEnumImport = 1,
        [Description("Εξαγωγή")]
        InventoryActionEnumExport = 2,
        [Description("Αρνητική Εισαγωγή")]
        InventoryActionEnumNegativeImport = 3,
        [Description("Αρνητική Εξαγωγή")]
        InventoryActionEnumNegativeExport = 4
    }
    public enum InventoryValueActionEnum
    {
        [Description("Καμία Μεταβολή")]
        InventoryValueActionEnumNoChange = 0,
        [Description("Αυξηση")]
        InventoryValueActionEnumIncrease = 1,
        [Description("Μείωση")]
        InventoryValueActionEnumDecrease = 2,
        [Description("Αρνητική Αύξηση")]
        InventoryValueActionEnumNegativeIncrease = 3,
        [Description("Αρνητική Μείωση")]
        InventoryValueActionEnumNegativeDecrease = 4
    }
    public enum FinActionsEnum
    {
        [Description("Καμία Μεταβολή")]
        FinActionsEnumNoChange = 0,
        [Description("Χρέωση")]
        FinActionsEnumDebit = 1,
        [Description("Πίστωση")]
        FinActionsEnumCredit = 2,
        [Description("Αρνητική Χρέωση")]
        FinActionsEnumNegativeDebit = 3,
        [Description("Αρνητική Πίστωση")]
        FinActionsEnumNegativeCredit = 4
    }

    public enum PriceTypeEnum
    {
        [Description("Καθαρή Τιμή")]
        PriceTypeEnumNetto = 1,
        [Description("Μικτή Τιμή")]
        PriceTypeEnumBrutto = 2,
        [Description("Τελ.Τιμή Αγοράς")]
        PriceTypeEnumLastBuy = 3,
        [Description("Τιμή Κόστους")]
        PriceTypeEnumCost = 4
    }

    public enum WarehouseItemCodeTypeEnum
    {
        [Description("Κωδικό")]
        CodeTypeEnumCode = 1,
        [Description("Barcode")]
        CodeTypeEnumBarcode = 2,
        [Description("Κωδ.Προμ.")]
        CodeTypeEnumSupplierCode = 3
    }

    public enum WarehouseItemCodeUsedUnitEnum
    {
        [Description("Κύρια")]
        CodeUsedUnitEnumMain = 1,
        [Description("Δευτερεύουσα")]
        CodeUsedUnitEnumSecondary = 2,
        [Description("Αγορών")]
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
    public enum InfoEntityActionEnum
    {
        [Description("No change")]
        InfoEntityActionEnumNoChange = 1,
        [Description("Increase")]
        InfoEntityActionEnumIncrease = 2,
        [Description("Decrease")]
        InfoEntityActionEnumDecrease = 3,
    }
    public enum PictureEntityEnum
    {
        [Description("Product")]
        PictureEntityEnumProduct = 1
        
    }
}
