using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GrKouk.InfoSystem.Domain.FinConfig;

namespace GrKouk.InfoSystem.Dtos.WebDtos.WarehouseTransactions
{
    public class WarehouseTransListDto
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:ddd dd MMM yyyy}")]
        [Display(Name = "Date")]
        public DateTime TransDate { get; set; }

        public int TransWarehouseDocSeriesId { get; set; }
        [Display(Name = "Series")]
        public string TransWarehouseDocSeriesName { get; set; }
        [Display(Name = "Series")]
        public string TransWarehouseDocSeriesCode { get; set; }
        [Display(Name = "Ref.")]
        public string TransRefCode { get; set; }
        [Display(Name = "Section")]
        public string SectionCode { get; set; }

        public int MaterialId { get; set; }
        [Display(Name = "Material")]
        public string MaterialName { get; set; }
      
        public WarehouseTransactionTypeEnum TransactionType { get; set; }

        public WarehouseInventoryTransTypeEnum InventoryAction { get; set; }
        public WarehouseValueTransTypeEnum InventoryValueAction { get; set; }
       
        public double Quontity1 { get; set; }
        public double Quontity2 { get; set; }
       
        public decimal UnitPrice { get; set; }
        public decimal AmountFpa { get; set; }
        public decimal AmountNet { get; set; }
        public decimal AmountDiscount { get; set; }

        public decimal TotalAmount
        {
            get => AmountNet + AmountFpa - AmountDiscount;

        }
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Import Value")]
        public decimal DebitAmount
        {
            get => (TransactionType.Equals(WarehouseTransactionTypeEnum.WarehouseTransactionTypeImport) ? TotalAmount : 0);

        }
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Export Value")]
        public decimal CreditAmount => (TransactionType.Equals(WarehouseTransactionTypeEnum.WarehouseTransactionTypeExport)
            ? TotalAmount
            : 0);

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Import Units")]
        public decimal ImportUnits
        {
            get => (TransactionType.Equals(WarehouseTransactionTypeEnum.WarehouseTransactionTypeImport) ?(decimal) Quontity1 : 0);

        }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Export Units")]
        public decimal ExportUnits => (TransactionType.Equals(WarehouseTransactionTypeEnum.WarehouseTransactionTypeExport)
            ? (decimal ) Quontity1
            : 0);
        public int CompanyId { get; set; }
        [Display(Name = "Company")]
        public string CompanyCode { get; set; }
    }
}
