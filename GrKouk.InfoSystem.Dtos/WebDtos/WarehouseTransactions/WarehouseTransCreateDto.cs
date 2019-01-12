﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.InfoSystem.Domain.Shared;

namespace GrKouk.InfoSystem.Dtos.WebDtos.WarehouseTransactions
{
    public class WarehouseTransCreateDto
    {
      //  public int Id { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime TransDate { get; set; }
        public int TransWarehouseDocSeriesId { get; set; }
        [Display(Name = "Ref")]
        public string TransRefCode { get; set; }
        [Display(Name = "Material")]
        public int MaterialId { get; set; }

        public int FiscalPeriodId { get; set; }
        public WarehouseTransactionTypeEnum TransactionType { get; set; }
        public InventoryActionEnum InventoryAction { get; set; }
        public InventoryValueActionEnum InventoryValueAction { get; set; }
        public int SectionId { get; set; }
        [Display(Name = "Main Unit")]
        public int PrimaryUnitId { get; set; }
        [Display(Name = "Sec.Unit")]
        public int SecondaryUnitId { get; set; }
        public decimal UnitFactor { get; set; }
        [Display(Name = "Q1")]
        public double Quontity1 { get; set; }
        [Display(Name = "Q2")]
        public double Quontity2 { get; set; }
        [Display(Name = "Fpa Rate")]
        public decimal FpaRate { get; set; }
        [Display(Name = "Disc.Rate")]
        public decimal DiscountRate { get; set; }
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }
        [Display(Name = "Vat Amount")]
        public decimal AmountFpa { get; set; }
        [Display(Name = "Amount Net")]
        public decimal AmountNet { get; set; }
        [Display(Name = "Amount Desc")]
        public decimal AmountDiscount { get; set; }
        [MaxLength(500)]
        [Display(Name = "Description")]
        public string Etiology { get; set; }
        [Display(Name = "Company")]
        public int CompanyId { get; set; }
      
        [Timestamp]
        public byte[] Timestamp { get; set; }




    }
}
