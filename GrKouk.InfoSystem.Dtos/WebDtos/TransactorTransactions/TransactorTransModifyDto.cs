﻿using System;
using System.ComponentModel.DataAnnotations;
using GrKouk.InfoSystem.Definitions;
using GrKouk.InfoSystem.Domain.FinConfig;

namespace GrKouk.InfoSystem.Dtos.WebDtos.TransactorTransactions
{
    public class TransactorTransModifyDto
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Trans Date")]
        public DateTime TransDate { get; set; }
        [Display(Name = "Doc Series")]
        public int TransTransactorDocSeriesId { get; set; }

        public int TransTransactorDocTypeId { get; set; }
        [Display(Name = "Reference Code")]
        public string TransRefCode { get; set; }
        [Display(Name = "Transactor")]
        public int TransactorId { get; set; }
        [Display(Name = "Section")]
        public int SectionId { get; set; }
        public int CreatorId { get; set; }
        [Display(Name = "Fiscal Period")]
        public int FiscalPeriodId { get; set; }

        public FinActionsEnum FinancialAction { get; set; }
        [Display(Name = "VAT Rate")]
        public decimal FpaRate { get; set; }
        [Display(Name = "Discount Rate")]
        public decimal DiscountRate { get; set; }
        [Display(Name = "VAT Amount")]
        public decimal AmountFpa { get; set; }
        [Display(Name = "Net")]
        public decimal AmountNet { get; set; }
        [Display(Name = "Discount")]
        public decimal AmountDiscount { get; set; }
        [Display(Name = "Sum Amount")]
        public decimal AmountSum => (AmountNet + AmountFpa - AmountDiscount);
        //public decimal TransFpaAmount { get; set; }
        //public decimal TransNetAmount { get; set; }
        //public decimal TransDiscountAmount { get; set; }

        [MaxLength(500)]
        [Display(Name = "Etiology")]
        public string Etiology { get; set; }
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}