using System;
using System.ComponentModel.DataAnnotations;
using GrKouk.InfoSystem.Domain.FinConfig;

namespace GrKouk.InfoSystem.Domain.Shared
{
    public class CustomerTransaction
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime TransDate { get; set; }

        public int TransCustomerDocSeriesId { get; set; }
        public virtual TransCustomerDocSeriesDef TransCustomerDocSeries { get; set; }

        public int TransCustomerDocTypeId { get; set; }
        public virtual TransCustomerDocTypeDef TransCustomerDocType { get; set; }


        /// <summary>
        /// Αριθμός Παραστατικού
        /// </summary>
        public string TransRefCode { get; set; }

        public int SectionId { get; set; }
        public virtual Section Section { get; set; }

        public int CustomerId { get; set; }
        public virtual Transactor Customer { get; set; }

        public int FiscalPeriodId { get; set; }
        public virtual FiscalPeriod FiscalPeriod { get; set; }
        /// <summary>
        /// Debit or Credit
        /// </summary>
        public FinancialTransactionTypeEnum TransactionType { get; set; }

        /// <summary>
        /// Ανάλογα με το sectionid το πεδίο αυτό περιέχει το id του αντικειμένου που ορίζει το sectionID 
        /// </summary>
        public int CreatorId { get; set; }

        public int FpaDefId { get; set; }
        public virtual FpaDef FpaDef { get; set; }
        public Single FpaRate { get; set; }

        public decimal AmountFpa { get; set; }
        public decimal AmountNet { get; set; }
        public decimal AmountDiscount { get; set; }
        [MaxLength(500)]
        public string Etiology { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}