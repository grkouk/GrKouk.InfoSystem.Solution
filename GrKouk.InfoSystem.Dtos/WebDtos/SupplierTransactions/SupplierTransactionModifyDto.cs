using System;
using System.ComponentModel.DataAnnotations;
using GrKouk.InfoSystem.Domain.FinConfig;

namespace GrKouk.InfoSystem.Dtos.WebDtos.SupplierTransactions
{
    public class SupplierTransactionModifyDto
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime TransDate { get; set; }

        public int TransSupplierDocSeriesId { get; set; }
        //public virtual TransSupplierDocSeriesDef TransSupplierDocSeries { get; set; }

        public int TransSupplierDocTypeId { get; set; }



        /// <summary>
        /// Αριθμός Παραστατικού
        /// </summary>
        public string TransRefCode { get; set; }

        public int SectionId { get; set; }


        public int SupplierId { get; set; }


        public int FiscalPeriodId { get; set; }

        /// <summary>
        /// Debit or Credit
        /// </summary>
        public FinancialTransactionTypeEnum TransactionType { get; set; }

        public FinancialTransTypeEnum TransactionAction { get; set; }
       
        public Single FpaRate { get; set; }
        [DataType(DataType.Currency)]
        public decimal AmountFpa { get; set; }
        [DataType(DataType.Currency)]
        public decimal AmountNet { get; set; }
        [DataType(DataType.Currency)]
        public decimal AmountDiscount { get; set; }
        [MaxLength(500)]
        public string Etiology { get; set; }

        public int CompanyId { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
