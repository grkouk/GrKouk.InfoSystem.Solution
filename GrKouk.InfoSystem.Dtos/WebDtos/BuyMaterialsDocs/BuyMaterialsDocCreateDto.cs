using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.InfoSystem.Domain.Shared;

namespace GrKouk.InfoSystem.Dtos.WebDtos.BuyMaterialsDocs
{
    public class BuyMaterialsDocCreateDto
    {
        private ICollection<BuyMaterialsDocLine> _buyDocLines;

        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime TransDate { get; set; }
       
        public string TransRefCode { get; set; }

        public int SectionId { get; set; }
        public string  SectionCode { get; set; }

        public int SupplierId { get; set; }
        public  string SupplierName { get; set; }

        public int FiscalPeriodId { get; set; }
        public  string FiscalPeriodCode { get; set; }

        public int MaterialDocSeriesId { get; set; }
        public  string MaterialDocSeriesCode { get; set; }
        public string MaterialDocSeriesName { get; set; }

        public int MaterialDocTypeId { get; set; }
        public  string MaterialDocTypeCode { get; set; }
        public string MaterialDocTypeName { get; set; }

        public decimal AmountFpa { get; set; }
        public decimal AmountNet { get; set; }
        public decimal AmountDiscount { get; set; }

        [MaxLength(500)]
        public string Etiology { get; set; }
        public int CompanyId { get; set; }
        public string CompanyCode { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
        public virtual ICollection<BuyMaterialsDocLine> BuyDocLines
        {
            get { return _buyDocLines ?? (_buyDocLines = new List<BuyMaterialsDocLine>()); }
            set { _buyDocLines = value; }
        }
    }
}
