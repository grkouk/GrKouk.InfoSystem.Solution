using System;

namespace GrKouk.InfoSystem.Dtos.WebDtos.BuyDocuments
{
    public class BuyDocLineAjaxDto
    {
        public int MaterialId { get; set; }
        public double Q1 { get; set; }
        public double Q2 { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public Single DiscountRate { get; set; }
        public int MainUnitId { get; set; }
        public int SecUnitId { get; set; }
        public Single Factor { get; set; }
        public Single FpaRate { get; set; }

    }
}
