using GrKouk.InfoSystem.Domain.FinConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GrKouk.InfoSystem.Domain.Shared
{
    public class BuyDocument
    {
        private ICollection<BuyDocLine> _buyDocLines;

        public int Id { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime TransDate { get; set; }
      
        public string TransRefCode { get; set; }
        [Required]
        public int SectionId { get; set; }
        public virtual Section Section { get; set; }
        [Required]
        public int TransactorId { get; set; }
        public virtual Transactor Transactor { get; set; }

        public int FiscalPeriodId { get; set; }
        public virtual FiscalPeriod FiscalPeriod { get; set; }
        [Required]
        public int BuyDocSeriesId { get; set; }
        public virtual BuyDocSeriesDef BuyDocSeries { get; set; }
        [Required]
        public int BuyDocTypeId { get; set; }
        public virtual BuyDocTypeDef BuyDocType { get; set; }

        public decimal AmountFpa { get; set; }
        public decimal AmountNet { get; set; }
        public decimal AmountDiscount { get; set; }

        [MaxLength(500)]
        public string Etiology { get; set; }

        [Required]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
        public virtual ICollection<BuyDocLine> BuyDocLines
        {
            get { return _buyDocLines ?? (_buyDocLines = new List<BuyDocLine>()); }
            set { _buyDocLines = value; }
        }
    }
}
