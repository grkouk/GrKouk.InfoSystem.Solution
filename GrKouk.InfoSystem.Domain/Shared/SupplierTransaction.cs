﻿using System;
using System.ComponentModel.DataAnnotations;
using GrKouk.InfoSystem.Domain.FinConfig;

namespace GrKouk.InfoSystem.Domain.Shared
{
    public class SupplierTransaction
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime TransDate { get; set; }

        public int TransSupplierDocSeriesId { get; set; }
        public virtual TransSupplierDocSeriesDef TransSupplierDocSeries { get; set; }

        public int TransSupplierDocTypeId { get; set; }
        public virtual TransSupplierDocTypeDef TransSupplierDocType { get; set; }
     
        public string TransRefCode { get; set; }

        public int SectionId { get; set; }
        public virtual Section Section { get; set; }

        public int SupplierId { get; set; }
        public virtual Transactor Supplier { get; set; }

        public int FiscalPeriodId { get; set; }
        public virtual FiscalPeriod FiscalPeriod { get; set; }
        /// <summary>
        /// Debit or Credit
        /// </summary>
        public FinancialTransactionTypeEnum TransactionType { get; set; }

        public FinActionsEnum FinancialAction { get; set; }
       
        public int CreatorId { get; set; }

      
        public decimal FpaRate { get; set; }
        public decimal DiscountRate { get; set; }   
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