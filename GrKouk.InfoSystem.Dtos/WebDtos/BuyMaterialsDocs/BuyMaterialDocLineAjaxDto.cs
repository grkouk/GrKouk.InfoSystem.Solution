using System;
using System.Collections.Generic;
using System.Text;

namespace GrKouk.InfoSystem.Dtos.WebDtos.BuyMaterialsDocs
{
    public class BuyMaterialDocLineAjaxDto
    {
        public int MaterialId { get; set; }
        public double Q1 { get; set; }
        public double Q2 { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public Single DiscountRate { get; set; }
        public int MainMeasureUnitId { get; set; }
        public int SecondaryMeasureUnitId { get; set; }
        public double SecondaryUnitToMainRate { get; set; }
        public Single FpaRate { get; set; }

    }
}
