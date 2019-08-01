using System;
using System.Collections.Generic;
using System.Text;

namespace GrKouk.InfoSystem.Dtos.WebDtos.CashRegister
{
   public class CashRegCatProductListDto
    {
        public int Id { get; set; }
        public string ClientProfileName { get; set; }
        public string CompanyCode { get; set; }
        public string CashRegCategoryName { get; set; }
        public string WarehouseItemName { get; set; }

    }
}
