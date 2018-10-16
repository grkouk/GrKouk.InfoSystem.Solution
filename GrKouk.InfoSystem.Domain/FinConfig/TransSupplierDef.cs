using System.ComponentModel.DataAnnotations;
using GrKouk.InfoSystem.Domain.Shared;

namespace GrKouk.InfoSystem.Domain.FinConfig
{
    public class TransSupplierDef
    {
        public int Id { get; set; }
        [MaxLength(15)]

        public string Code { get; set; }
        [MaxLength(200)]

        public string Name { get; set; }
        [Display(Name = "Ενεργό")]
        public bool Active { get; set; }

        [Display(Name = "Credit Transaction Id")]
        public int CreditTransId { get; set; }
        public virtual FinancialMovement CreditTrans { get; set; }

        [Display(Name = "Debit Transaction Id")]
        public int DebitTransId { get; set; }
        public virtual FinancialMovement DebitTrans { get; set; }

        [Display(Name = "Κίνηση Τζίρου")]
        public int TurnOverTransId { get; set; }
        public virtual FinancialMovement TurnOverTrans { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}