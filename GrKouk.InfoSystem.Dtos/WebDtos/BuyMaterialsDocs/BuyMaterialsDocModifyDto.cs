using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GrKouk.InfoSystem.Domain.Shared;

namespace GrKouk.InfoSystem.Dtos.WebDtos.BuyMaterialsDocs
{
    public class BuyMaterialsDocModifyDto
    {
        private ICollection<BuyMaterialsDocLineModifyDto> _buyDocLines;

        public int Id { get; set; }

        [DataType(DataType.Date)] public DateTime TransDate { get; set; }

        public string TransRefCode { get; set; }

        public int SectionId { get; set; }
        public string SectionCode { get; set; }

        public int SupplierId { get; set; }
        public string SupplierName { get; set; }

        public int FiscalPeriodId { get; set; }
        public string FiscalPeriodCode { get; set; }

        public int MaterialDocSeriesId { get; set; }
        public string MaterialDocSeriesCode { get; set; }
        public string MaterialDocSeriesName { get; set; }

        public int MaterialDocTypeId { get; set; }
        public string MaterialDocTypeCode { get; set; }
        public string MaterialDocTypeName { get; set; }
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

        public virtual ICollection<BuyMaterialsDocLineModifyDto> BuyDocLines
        {
            get { return _buyDocLines ?? (_buyDocLines = new List<BuyMaterialsDocLineModifyDto>()); }
            set { _buyDocLines = value; }
        }
    }
}
    