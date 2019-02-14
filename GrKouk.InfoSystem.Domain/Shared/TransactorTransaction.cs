using System;
using System.ComponentModel.DataAnnotations;
using GrKouk.InfoSystem.Definitions;
using GrKouk.InfoSystem.Domain.FinConfig;

namespace GrKouk.InfoSystem.Domain.Shared
{
    public class TransactorTransaction
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime TransDate { get; set; }

        public int TransTransactorDocSeriesId { get; set; }
        public virtual TransTransactorDocSeriesDef TransTransactorDocSeries { get; set; }

        public int TransTransactorDocTypeId { get; set; }
        public virtual TransTransactorDocTypeDef TransTransactorDocType { get; set; }

        public string TransRefCode { get; set; }
        public int TransactorId { get; set; }
        public virtual Transactor Transactor { get; set; }

        public int SectionId { get; set; }
        public virtual Section Section { get; set; }
        public int CreatorId { get; set; }

        public int FiscalPeriodId { get; set; }
        public virtual FiscalPeriod FiscalPeriod { get; set; }
      
        public FinActionsEnum FinancialAction { get; set; }

        public decimal FpaRate { get; set; }
        public decimal DiscountRate { get; set; }

        public decimal AmountFpa { get; set; }
        public decimal AmountNet { get; set; }
        public decimal AmountDiscount { get; set; }

        public decimal TransFpaAmount { get; set; }
        public decimal TransNetAmount { get; set; }
        public decimal TransDiscountAmount { get; set; }

        [MaxLength(500)]
        public string Etiology { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

    }
}