using System.ComponentModel.DataAnnotations;
using GrKouk.InfoSystem.Domain.Shared;

namespace GrKouk.InfoSystem.Domain.FinConfig
{
    public class TransSupplierDef
    {
        public int Id { get; set; }
        /// <summary>
        /// Κωδικός της κίνησης προμηθευτή
        /// </summary>
        [MaxLength(15)]
        public string Code { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        [Display(Name = "Ενεργό")]
        public bool Active { get; set; }
        /// <summary>
        /// Τύπος κίνησης για την Πίστωση
        /// </summary>
        [Display(Name = "Credit Transaction Id")]
        public int CreditTransId { get; set; }
        public virtual FinancialMovement CreditTrans { get; set; }
        /// <summary>
        /// ΄Κωδικός κίνησης για την Χρέωση
        /// </summary>
        [Display(Name = "Debit Transaction Id")]
        public int DebitTransId { get; set; }
        public virtual FinancialMovement DebitTrans { get; set; }
        /// <summary>
        /// Κωδικός κίνησης για τον Τζίρο
        /// </summary>
        [Display(Name = "Κίνηση Τζίρου")]
        public int TurnOverTransId { get; set; }
        public virtual FinancialMovement TurnOverTrans { get; set; }
        /// <summary>
        /// Εταιρεία για την οποία είναι ενεργή η κίνηση
        /// </summary>
        public int CompanyId { get; set; }
       
        public virtual Company Company { get; set; }
         [Display(Name = "Default Series")]
        public int TransSupplierDefaultDocSeriesId { get; set; }
        public virtual TransSupplierDocSeriesDef TransSupplierDefaultDocSeries { get; set; }
    }
}