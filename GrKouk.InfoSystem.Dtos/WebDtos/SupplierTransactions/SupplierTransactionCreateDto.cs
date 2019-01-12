using GrKouk.InfoSystem.Domain.FinConfig;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Runtime.Serialization;

namespace GrKouk.InfoSystem.Dtos.WebDtos.SupplierTransactions
{
    public class SupplierTransactionCreateDto
    {

        [DataType(DataType.Date)]
        
        public DateTime TransDate { get; set; }

        public int TransSupplierDocSeriesId { get; set; }
        public virtual TransSupplierDocSeriesDef TransSupplierDocSeries { get; set; }

        public int TransSupplierDocTypeId { get; set; }
        public string TransRefCode { get; set; }

        public int SectionId { get; set; }
       

        public int SupplierId { get; set; }
       

        public int FiscalPeriodId { get; set; }

       
        public FinancialTransactionTypeEnum TransactionType { get; set; }


        [Display(Name = "VAT%")]
        public Single FpaRate { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "VAT Amount")]
        public decimal AmountFpa { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Net Amount")]
        public decimal AmountNet { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Discount Amount")]
        public decimal AmountDiscount { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Sum Amount")]
        public decimal AmountSum => (AmountNet + AmountFpa - AmountDiscount);

        [MaxLength(500)]
        public string Etiology { get; set; }

        public int CompanyId { get; set; }

    }

}
