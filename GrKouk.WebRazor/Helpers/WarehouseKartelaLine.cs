using System;
using System.ComponentModel.DataAnnotations;

namespace GrKouk.WebRazor.Helpers
{
    public class WarehouseKartelaLine
    {
        [DataType(DataType.Date)]

        public DateTime TransDate { get; set; }

        public string MaterialName { get; set; }
        public string DocSeriesCode { get; set; }
        public decimal ImportVolume { get; set; }
        public decimal ExportVolume { get; set; }
        public decimal ImportValue { get; set; }
        public decimal ExportValue { get; set; }
        public decimal RunningTotalVolume { get; set; }
        public decimal RunningTotalValue { get; set; }
    }
}