using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GrKouk.InfoSystem.Domain.FinConfig;

namespace GrKouk.InfoSystem.Domain.Shared
{
    public class SellDocument
    {
        private ICollection<SellDocLine> _sellDocLines;

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
        public int SellDocSeriesId { get; set; }
        public virtual SellDocSeriesDef SellDocSeries { get; set; }
        [Required]
        public int SellDocTypeId { get; set; }
        public virtual SellDocTypeDef SellDocType { get; set; }

        public decimal AmountFpa { get; set; }
        public decimal AmountNet { get; set; }
        public decimal AmountDiscount { get; set; }

        [MaxLength(500)]
        public string Etiology { get; set; }

        [Required]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
        public virtual ICollection<SellDocLine> SellDocLines
        {
            get { return _sellDocLines ?? (_sellDocLines = new List<SellDocLine>()); }
            set { _sellDocLines = value; }
        }
    }
}