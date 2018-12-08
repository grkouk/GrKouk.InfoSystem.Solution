﻿using GrKouk.InfoSystem.Domain.FinConfig;
using System;
using System.ComponentModel.DataAnnotations;

namespace GrKouk.InfoSystem.Dtos.WebDtos.SupplierTransactions
{
    public class SupplierTransactionCreateDto
    {

        [DataType(DataType.Date)]
        public DateTime TransDate { get; set; }

        public int TransSupplierDocSeriesId { get; set; }
        public virtual TransSupplierDocSeriesDef TransSupplierDocSeries { get; set; }

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

        

        public int FpaDefId { get; set; }
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

    }

}
