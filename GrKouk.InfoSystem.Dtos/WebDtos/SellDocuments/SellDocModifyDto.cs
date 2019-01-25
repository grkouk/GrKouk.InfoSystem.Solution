﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GrKouk.InfoSystem.Dtos.WebDtos.BuyMaterialsDocs;

namespace GrKouk.InfoSystem.Dtos.WebDtos.SellDocuments
{
   public class SellDocModifyDto
    {
        private ICollection<SellDocLineModifyDto> _sellDocLines;

        public int Id { get; set; }

        [DataType(DataType.Date)] public DateTime TransDate { get; set; }

        public string TransRefCode { get; set; }

        public int SectionId { get; set; }
        public string SectionCode { get; set; }

        public int TransactorId { get; set; }
        public string TransactorName { get; set; }

        public int FiscalPeriodId { get; set; }
        public string FiscalPeriodCode { get; set; }

        public int SellDocSeriesId { get; set; }
        public string SellDocSeriesCode { get; set; }
        public string SellDocSeriesName { get; set; }

        public int SellDocTypeId { get; set; }
        public string SellDocTypeCode { get; set; }
        public string SellDocTypeName { get; set; }
        [Display(Name = "Vat Amount")] public decimal AmountFpa { get; set; }
        [Display(Name = "Net Amount")] public decimal AmountNet { get; set; }
        [Display(Name = "Discount Amount")] public decimal AmountDiscount { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Sum")]
        public decimal AmountSum => (AmountNet + AmountFpa - AmountDiscount);

        [MaxLength(500)] public string Etiology { get; set; }
        public int CompanyId { get; set; }
        public string CompanyCode { get; set; }

        [Timestamp] public byte[] Timestamp { get; set; }

        public virtual ICollection<SellDocLineModifyDto> SellDocLines
        {
            get { return _sellDocLines ?? (_sellDocLines = new List<SellDocLineModifyDto>()); }
            set { _sellDocLines = value; }
        }
    }
}