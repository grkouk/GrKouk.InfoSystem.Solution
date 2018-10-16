using System.ComponentModel.DataAnnotations;
using GrKouk.InfoSystem.Domain.Shared;

namespace GrKouk.InfoSystem.Domain.FinConfig
{
    public class TransWarehouseDef
    {
        public int Id { get; set; }

        [MaxLength(15)] public string Code { get; set; }
        [MaxLength(200)] public string Name { get; set; }
        [Display(Name = "Ενεργό")] public bool Active { get; set; }

        [Display(Name = "Ποσότητα Εισαγωγών")]
        public int VolImportsTransId { get; set; }
        public virtual FinancialMovement VolImportsTrans { get; set; }

        [Display(Name = "Ποσότητα Εξαγωγών")]
        public int VolExportsTransId { get; set; }
        public virtual FinancialMovement VolExportsTrans { get; set; }

        [Display(Name = "Ποσότητα Τιμολογημένων Εξαγωγών")]
        public int VolInvoicedExportsTransId { get; set; }

        public virtual FinancialMovement VolInvoicedExportsTrans { get; set; }

        [Display(Name = "Ποσότητα Τιμολογημένων Εισαγωγών")]
        public int VolInvoicedImportsTransId { get; set; }

        public virtual FinancialMovement VolInvoicedImportsTrans { get; set; }

        [Display(Name = "Αξία Εισαγωγών")]
        public int AmtImportsTransId { get; set; }
        public virtual FinancialMovement AmtImportsTrans { get; set; }

        [Display(Name = "Αξία Εξαγωγών")]
        public int AmtExportsTransId { get; set; }
        public virtual FinancialMovement AmtExportsTrans { get; set; }

        [Display(Name = "Αξία Τιμολογημένων Εξαγωγών")]
        public int AmtInvoicedExportsTransId { get; set; }

        public virtual FinancialMovement AmtInvoicedExportsTrans { get; set; }

        [Display(Name = "Αξία Τιμολογημένων Εισαγωγών")]
        public int AmtInvoicedImportsTransId { get; set; }

        public virtual FinancialMovement AmtInvoicedImportsTrans { get; set; }

        [Display(Name = "Ποσότητα Αγορών")]
        public int VolBuyTransId { get; set; }
        public virtual FinancialMovement VolBuyTrans { get; set; }

        [Display(Name = "Αξία Αγορών")]
        public int AmtBuyTransId { get; set; }
        public virtual FinancialMovement AmtBuyTrans { get; set; }

        [Display(Name = "Ποσότητα Πωλήσεων")]
        public int VolSellTransId { get; set; }
        public virtual FinancialMovement VolSellTrans { get; set; }

        [Display(Name = "Αξία Πωλήσεων")]
        public int AmtSellTransId { get; set; }
        public virtual FinancialMovement AmtSellTrans { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}