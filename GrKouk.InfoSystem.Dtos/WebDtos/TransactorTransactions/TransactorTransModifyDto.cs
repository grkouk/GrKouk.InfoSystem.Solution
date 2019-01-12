﻿using System;
using System.ComponentModel.DataAnnotations;
using GrKouk.InfoSystem.Domain.FinConfig;

namespace GrKouk.InfoSystem.Dtos.WebDtos.TransactorTransactions
{
    public class TransactorTransModifyDto
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime TransDate { get; set; }

        public int TransTransactorDocSeriesId { get; set; }

        public int TransTransactorDocTypeId { get; set; }

        public string TransRefCode { get; set; }
        public int TransactorId { get; set; }

        public int SectionId { get; set; }
        public int CreatorId { get; set; }

        public int FiscalPeriodId { get; set; }

        public FinActionsEnum FinancialAction { get; set; }

        public decimal FpaRate { get; set; }
        public decimal DiscountRate { get; set; }

        public decimal AmountFpa { get; set; }
        public decimal AmountNet { get; set; }
        public decimal AmountDiscount { get; set; }
        [Display(Name = "Sum Amount")]
        public decimal AmountSum => (AmountNet + AmountFpa - AmountDiscount);
        //public decimal TransFpaAmount { get; set; }
        //public decimal TransNetAmount { get; set; }
        //public decimal TransDiscountAmount { get; set; }

        [MaxLength(500)]
        public string Etiology { get; set; }

        public int CompanyId { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}