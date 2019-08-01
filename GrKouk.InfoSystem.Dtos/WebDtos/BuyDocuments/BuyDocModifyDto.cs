using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GrKouk.InfoSystem.Dtos.WebDtos.BuyDocuments
{
    public class BuyDocModifyDto
    {
        private ICollection<BuyDocLineModifyDto> _buyDocLines;

        public int Id { get; set; }

        [DataType(DataType.Date)] public DateTime TransDate { get; set; }

        public string TransRefCode { get; set; }

        public int SectionId { get; set; }
        public string SectionCode { get; set; }

        public int TransactorId { get; set; }
        public string TransactorName { get; set; }

        public int FiscalPeriodId { get; set; }
        public string FiscalPeriodCode { get; set; }

        public int BuyDocSeriesId { get; set; }
        public string BuyDocSeriesCode { get; set; }
        public string BuyDocSeriesName { get; set; }

        public int BuyDocTypeId { get; set; }
        public string BuyDocTypeCode { get; set; }
        public string BuyDocTypeName { get; set; }
        [Display(Name = "Vat Amount")] public decimal AmountFpa { get; set; }
        [Display(Name = "Net Amount")] public decimal AmountNet { get; set; }
        [Display(Name = "Discount Amount")] public decimal AmountDiscount { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Sum")]
        public decimal AmountSum => (AmountNet + AmountFpa - AmountDiscount);

        [MaxLength(500)] public string Etiology { get; set; }
        public int CompanyId { get; set; }
        public string CompanyCode { get; set; }

        public int PaymentMethodId { get; set; }

        [Timestamp] public byte[] Timestamp { get; set; }

        public virtual ICollection<BuyDocLineModifyDto> BuyDocLines
        {
            get { return _buyDocLines ?? (_buyDocLines = new List<BuyDocLineModifyDto>()); }
            set { _buyDocLines = value; }
        }
    }
}
    