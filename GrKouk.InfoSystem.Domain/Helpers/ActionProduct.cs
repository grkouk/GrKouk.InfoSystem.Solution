using GrKouk.InfoSystem.Definitions;
using GrKouk.InfoSystem.Domain.FinConfig;

namespace GrKouk.InfoSystem.Domain.Helpers
{
    /// <summary>
    /// Based on the financialMovement action has amounts and movement types
    /// </summary>
    public class ActionProduct
    {
        public FinancialTransactionTypeEnum FinancialTransactionType { get; set; }
        public WarehouseTransactionTypeEnum WarehouseTransactionTypeCode { get; set; }
        public decimal NetAmount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public double Quontity1 { get; set; }
        public double Quontity2 { get; set; }
    }
}
