using GrKouk.InfoSystem.Domain.FinConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GrKouk.InfoSystem.Domain.Shared
{
    public class BuyMaterialsDocument
    {
        private ICollection<BuyMaterialsDocLine> _buyDocLines;

        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime TransDate { get; set; }
        /// <summary>
        /// Αριθμός Παραστατικού
        /// </summary>
        public string TransRefCode { get; set; }

        public int SectionId { get; set; }
        public virtual Section Section { get; set; }

        public int SupplierId { get; set; }
        public virtual Transactor Supplier { get; set; }

        public int FiscalPeriodId { get; set; }
        public virtual FiscalPeriod FiscalPeriod { get; set; }

        public int MaterialDocSeriesId { get; set; }
        public virtual BuyMaterialDocSeriesDef MaterialDocSeries { get; set; }

        public int MaterialDocTypeId { get; set; }
        public virtual BuyMaterialDocTypeDef MaterialDocType { get; set; }

        public decimal AmountFpa { get; set; }
        public decimal AmountNet { get; set; }
        public decimal AmountDiscount { get; set; }

        [MaxLength(500)]
        public string Etiology { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }



        [Timestamp]
        public byte[] Timestamp { get; set; }
        public virtual ICollection<BuyMaterialsDocLine> BuyDocLines
        {
            get { return _buyDocLines ?? (_buyDocLines = new List<BuyMaterialsDocLine>()); }
            set { _buyDocLines = value; }
        }
    }
}
