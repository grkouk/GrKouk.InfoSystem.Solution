using System;

namespace GrKouk.InfoSystem.Dtos
{
    public class FinDiaryTransactionCreateDto
    {
        public int Id { get; set; }

        public DateTime TransactionDate { get; set; }
        public string ReferenceCode { get; set; }
        public int TransactorId { get; set; }
        public int FinTransCategoryId { get; set; }
        public int CompanyId { get; set; }
        public int CostCentreId { get; set; }
        public int RevenueCentreId { get; set; }
        public string Description { get; set; }
        public int Kind { get; set; }
        public decimal AmountFpa { get; set; }
        public decimal AmountNet { get; set; }
    }
}