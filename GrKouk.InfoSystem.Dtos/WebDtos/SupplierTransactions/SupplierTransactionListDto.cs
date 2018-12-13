using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using GrKouk.InfoSystem.Domain.FinConfig;

namespace GrKouk.InfoSystem.Dtos.WebDtos.SupplierTransactions
{
    public class SupplierTransactionListDto
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:ddd dd MMM yyyy}")]
        [Display(Name = "Date")]
        public DateTime TransDate { get; set; }

        public int TransSupplierDocSeriesId { get; set; }
        public string TransSupplierSeriesName { get; set; }
        [Display(Name = "Series Code")]
        public string TransSupplierSeriesCode { get; set; }
        [Display(Name = "Ref Code")]
        public string TransRefCode { get; set; }

        [Display(Name = "Section")]
        public string SectionCode { get; set; }

        public int SupplierId { get; set; }
        [Display(Name = "Supplier")]
        public string SupplierName { get; set; }

        public FinancialTransactionTypeEnum TransactionType { get; set; }
        public decimal AmountFpa { get; set; }
        public decimal AmountNet { get; set; }
        public decimal AmountDiscount { get; set; }
        
        public decimal TotalAmount
        {
            get => AmountNet+AmountFpa-AmountDiscount;

        }
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Debit")]
        public decimal DebitAmount
        {
            get => (TransactionType.Equals(FinancialTransactionTypeEnum.FinancialTransactionTypeDebit) ?  TotalAmount:0);

        }
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Credit")]
        public decimal CreditAmount => (TransactionType.Equals(FinancialTransactionTypeEnum.FinancialTransactionTypeCredit)
                    ? TotalAmount
                    : 0);
        [Display(Name = "Company")]
        public string  CompanyCode { get; set; }
    }
}
