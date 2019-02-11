using System.ComponentModel.DataAnnotations;
using GrKouk.InfoSystem.Domain.Shared;

namespace GrKouk.InfoSystem.Domain.FinConfig
{
    public class TransWarehouseDef
    {
        public int Id { get; set; }

        [MaxLength(15)]
        public string Code { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        [Display(Name = "Ενεργό")] public bool Active { get; set; }

        [Display(Name = "Mat.Inventory")]
        public InventoryActionEnum MaterialInventoryAction { get; set; }
       
        [Display(Name = "Mat.InvValAct")]
        public InventoryValueActionEnum MaterialInventoryValueAction { get; set; }
        [Display(Name = "Mat.Invoiced Volume")]
        public InventoryActionEnum MaterialInvoicedVolumeAction { get; set; }
        [Display(Name = "Mat.Invoiced Value")]
        public InventoryValueActionEnum MaterialInvoicedValueAction { get; set; }
        [Display(Name = "Ser.InvAct")]
        public InventoryActionEnum ServiceInventoryAction { get; set; }
        [Display(Name = "Ser.InvValAct")]
        public InventoryValueActionEnum ServiceInventoryValueAction { get; set; }
        [Display(Name = "Exp.InvAct")]
        public InventoryActionEnum ExpenseInventoryAction { get; set; }
        [Display(Name = "Exp.InvValAct")]
        public InventoryValueActionEnum ExpenseInventoryValueAction { get; set; }
        [Display(Name = "Inc.InvAct")]
        public InventoryActionEnum IncomeInventoryAction { get; set; }
        [Display(Name = "Inc.InvValAct")]
        public InventoryValueActionEnum IncomeInventoryValueAction { get; set; }

        [Display(Name = "Fixed As.InvAct")]
        public InventoryActionEnum FixedAssetInventoryAction { get; set; }
        [Display(Name = "Fixed As.InvValAct")]
        public InventoryValueActionEnum FixedAssetInventoryValueAction { get; set; }

        //[Display(Name = "Ποσ Τιμ/νων Εξ")]
        //public InfoEntityActionEnum VolInvoicedExportsAction { get; set; }

        //[Display(Name = "Ποσ Τιμ/νων Εισ")]
        //public InfoEntityActionEnum VolInvoicedImportsAction { get; set; }
        
        //[Display(Name = "Αξία Τιμ/νων Εξ")]
        //public InfoEntityActionEnum AmtInvoicedExportsAction { get; set; }

        //[Display(Name = "Αξία Τιμ/νων Εισ")]
        //public InfoEntityActionEnum AmtInvoicedImportsAction { get; set; }

        //[Display(Name = "Ποσ Αγορών")]
        //public InfoEntityActionEnum VolBuyAction { get; set; }
       

        //[Display(Name = "Αξία Αγορών")]
        //public InfoEntityActionEnum AmtBuyAction { get; set; }
       

        //[Display(Name = "Ποσ Πωλήσεων")]
        //public InfoEntityActionEnum VolSellAction { get; set; }
      

        //[Display(Name = "Αξία Πωλήσεων")]
        //public InfoEntityActionEnum AmtSellAction { get; set; }
       
        [Display(Name = "Default Series")]
        public int DefaultDocSeriesId { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}