namespace GrKouk.InfoSystem.Domain.FinConfig
{
    public enum DiaryTransactionsKindEnum
    {
        Expence = 1,
        Income = 2
    }
    public enum MaterialTypeEnum
    {
        MaterialTypeNormal=1,
        MaterialTypeSet=2,
        MaterialTypeComposed=3

    }
    /// <summary>
    /// Τύπος κίνησης
    /// 1=Χρεωση
    /// 2=Πίστωση
    /// 
    /// </summary>
    public enum FinancialTransactionTypeEnum
    {
        FinancialTransactionTypeDebit=1,
        FinancialTransactionTypeCredit=2
    }
    /// <summary>
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
}
