namespace GrKouk.InfoSystem.Domain.FinConfig
{
    public enum DiaryTransactionsKindEnum
    {
        Expence = 1,
        Income = 2
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
        WarehouseTransactionTypeImport = 1,
        WarehouseTransactionTypeExport = 2
    }
    /// <summary>
    /// Warehouse Transaction New Type Enum
    /// </summary>
    public enum WarehouseInventoryTransTypeEnum
    {
        WarehouseInventoryTransTypeEnumImport = 1,
        WarehouseInventoryTransTypeEnumExport = 2,
        WarehouseInventoryTransTypeEnumNegativeImport = 3,
        WarehouseInventoryTransTypeEnumNegativeExport = 4
    }
    public enum WarehouseValueTransTypeEnum
    {
        WarehouseValueTransTypeEnumIncrease = 1,
        WarehouseValueTransTypeEnumDecrease = 2,
        WarehouseValueTransTypeEnumNegativeIncrease = 3,
        WarehouseValueTransTypeEnumNegativeDecrease = 4
    }
    public enum FinancialTransTypeEnum
    {
        FinancialTransTypeDebit = 1,
        FinancialTransTypeCredit = 2,
        FinancialTransTypeNegativeDebit = 3,
        FinancialTransTypeNegativeCredit = 4
    }
}
