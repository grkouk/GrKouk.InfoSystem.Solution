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

        [Display(Name = "Mat.IA")]
        public InventoryActionEnum MaterialInventoryAction { get; set; }
        [Display(Name = "Mat.IVA")]
        public InventoryValueActionEnum MaterialInventoryValueAction { get; set; }
        [Display(Name = "Ser.IA")]
        public InventoryActionEnum ServiceInventoryAction { get; set; }
        [Display(Name = "Ser.IVA")]
        public InventoryValueActionEnum ServiceInventoryValueAction { get; set; }
        [Display(Name = "Exp.IA")]
        public InventoryActionEnum ExpenseInventoryAction { get; set; }
        [Display(Name = "Exp.IVA")]
        public InventoryValueActionEnum ExpenseInventoryValueAction { get; set; }
        [Display(Name = "Inc.IA")]
        public InventoryActionEnum IncomeInventoryAction { get; set; }
        [Display(Name = "Inc.IVA")]
        public InventoryValueActionEnum IncomeInventoryValueAction { get; set; }

        [Display(Name = "Fixed As.IA")]
        public InventoryActionEnum FixedAssetInventoryAction { get; set; }
        [Display(Name = "Fixed As.IVA")]
        public InventoryValueActionEnum FixedAssetInventoryValueAction { get; set; }

        [Display(Name = "ΠοσΕισαγωγών")]
        public int VolImportsTransId { get; set; }

        [Display(Name = "ΠοσΕξαγωγών")]
        public int VolExportsTransId { get; set; }

        [Display(Name = "Ποσ Τιμ/νων Εξ")]
        public int VolInvoicedExportsTransId { get; set; }

        public virtual FinancialMovement VolInvoicedExportsTrans { get; set; }

        [Display(Name = "Ποσ Τιμ/νων Εισ")]
        public int VolInvoicedImportsTransId { get; set; }

        public virtual FinancialMovement VolInvoicedImportsTrans { get; set; }

        [Display(Name = "Αξία Εισ")]
        public int AmtImportsTransId { get; set; }

        [Display(Name = "Αξία Εξα")]
        public int AmtExportsTransId { get; set; }

        [Display(Name = "Αξία Τιμ/νων Εξ")]
        public int AmtInvoicedExportsTransId { get; set; }

        public virtual FinancialMovement AmtInvoicedExportsTrans { get; set; }

        [Display(Name = "Αξία Τιμ/νων Εισ")]
        public int AmtInvoicedImportsTransId { get; set; }

        public virtual FinancialMovement AmtInvoicedImportsTrans { get; set; }

        [Display(Name = "Ποσ Αγορών")]
        public int VolBuyTransId { get; set; }
        public virtual FinancialMovement VolBuyTrans { get; set; }

        [Display(Name = "Αξία Αγορών")]
        public int AmtBuyTransId { get; set; }
        public virtual FinancialMovement AmtBuyTrans { get; set; }

        [Display(Name = "Ποσ Πωλήσεων")]
        public int VolSellTransId { get; set; }
        public virtual FinancialMovement VolSellTrans { get; set; }

        [Display(Name = "Αξία Πωλήσεων")]
        public int AmtSellTransId { get; set; }
        public virtual FinancialMovement AmtSellTrans { get; set; }
        [Display(Name = "Default Series")]
        public int DefaultDocSeriesId { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}