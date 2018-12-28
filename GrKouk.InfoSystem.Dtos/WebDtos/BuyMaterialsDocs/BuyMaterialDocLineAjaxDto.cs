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
        public int MainUnitId { get; set; }
        public int SecUnitId { get; set; }
        
        public Single FpaRate { get; set; }

    }
}
