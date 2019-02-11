using System;
using System.ComponentModel.DataAnnotations;
using GrKouk.InfoSystem.Definitions;

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

        public InventoryActionEnum InventoryAction { get; set; }
        public InventoryValueActionEnum InventoryValueAction { get; set; }
        //Added 2019-------------
        public InventoryActionEnum InvoicedVolumeAction { get; set; }
        public InventoryValueActionEnum InvoicedValueAction { get; set; }
        //----------------------
        public double Quontity1 { get; set; }
        public double Quontity2 { get; set; }
       
        public decimal UnitPrice { get; set; }
        public decimal AmountFpa { get; set; }
        public decimal AmountNet { get; set; }
        public decimal TransQ1 { get; set; }
        public decimal TransQ2 { get; set; }
        public decimal AmountDiscount { get; set; }
        public decimal TransFpaAmount { get; set; }
        public decimal TransNetAmount { get; set; }
        public decimal TransDiscountAmount { get; set; }
        public decimal TotalAmount
        {
            get => TransNetAmount + TransFpaAmount - TransDiscountAmount;

        }
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Import Value")]
        public decimal ImportAmount
        {
            get => (InventoryValueAction.Equals(InventoryValueActionEnum.InventoryValueActionEnumIncrease) ||
                    InventoryValueAction.Equals(InventoryValueActionEnum.InventoryValueActionEnumNegativeIncrease)
                ? TotalAmount
                : 0);

        }
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Export Value")]
        public decimal ExportAmount => (InventoryValueAction.Equals(InventoryValueActionEnum.InventoryValueActionEnumDecrease) ||
                                        InventoryValueAction.Equals(InventoryValueActionEnum.InventoryValueActionEnumNegativeDecrease)
            ? TotalAmount
            : 0);
       
       

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Import Units")]
        public decimal ImportUnits
        {
            get => (InventoryAction.Equals(InventoryActionEnum.InventoryActionEnumImport) ||
                    InventoryAction.Equals(InventoryActionEnum.InventoryActionEnumNegativeImport)
                ? TransQ1 
                : 0);

        }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Export Units")]
        public decimal ExportUnits => (InventoryAction.Equals(InventoryActionEnum.InventoryActionEnumExport) ||
                                       InventoryAction.Equals(InventoryActionEnum.InventoryActionEnumNegativeExport)
            ? TransQ1
            : 0);
      
        //-2019
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Invoiced Import Value")]
        public decimal InvoicedImportAmount
        {
            get => (InvoicedValueAction.Equals(InventoryValueActionEnum.InventoryValueActionEnumIncrease) ||
                    InvoicedValueAction.Equals(InventoryValueActionEnum.InventoryValueActionEnumNegativeIncrease)
                ? TotalAmount
                : 0);

        }
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Invoiced Export Value")]
        public decimal InvoicedExportAmount => (InvoicedValueAction.Equals(InventoryValueActionEnum.InventoryValueActionEnumDecrease) ||
                                                InvoicedValueAction.Equals(InventoryValueActionEnum.InventoryValueActionEnumNegativeDecrease)
            ? TotalAmount
            : 0);



        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Invoiced Import Units")]
        public decimal InvoicedImportUnits
        {
            get => (InvoicedVolumeAction.Equals(InventoryActionEnum.InventoryActionEnumImport) ||
                    InvoicedVolumeAction.Equals(InventoryActionEnum.InventoryActionEnumNegativeImport)
                ? TransQ1
                : 0);

        }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Invoiced Export Units")]
        public decimal InvoicedExportUnits => (InvoicedVolumeAction.Equals(InventoryActionEnum.InventoryActionEnumExport) ||
                                               InvoicedVolumeAction.Equals(InventoryActionEnum.InventoryActionEnumNegativeExport)
            ? TransQ1
            : 0);
        //------


        public int CompanyId { get; set; }
        [Display(Name = "Company")]
        public string CompanyCode { get; set; }
    }
}
