using System;
using System.ComponentModel.DataAnnotations;

namespace GrKouk.InfoSystem.Dtos.WebDtos.BuyMaterialsDocs
{
    public class BuyMaterialsDocModifyAjaxNoLinesDto
    {
        public int Id { get; set; }
        public DateTime TransDate { get; set; }

        public string TransRefCode { get; set; }

        public int SupplierId { get; set; }

        public int MaterialDocSeriesId { get; set; }
        public int MaterialDocTypeId { get; set; }
        public decimal AmountFpa { get; set; }
        public decimal AmountNet { get; set; }
        public decimal AmountDiscount { get; set; }

        public string Etiology { get; set; }
        public int FiscalPeriodId { get; set; }
        public int CompanyId { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}