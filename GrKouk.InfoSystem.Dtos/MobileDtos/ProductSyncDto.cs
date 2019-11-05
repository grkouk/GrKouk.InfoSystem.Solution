using System;
using System.Collections.Generic;
using System.Text;

namespace GrKouk.InfoSystem.Dtos.MobileDtos
{
    /// <summary>
    /// Product for client app sync (example for Tamiaki app)
    /// </summary>
    public class ProductSyncDto
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
        public decimal PriceNetto { get; set; }
        public decimal PriceBrutto { get; set; }
    }
}
