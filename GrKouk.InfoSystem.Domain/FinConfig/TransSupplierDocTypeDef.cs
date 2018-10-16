using System.ComponentModel.DataAnnotations;
using GrKouk.InfoSystem.Domain.Shared;

namespace GrKouk.InfoSystem.Domain.FinConfig
{
    public class TransSupplierDocTypeDef
    {
        public int Id { get; set; }

        [MaxLength(15)]
        public string Code { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }
        [Display(Name = "Ενεργό")]
        public bool Active { get; set; }

        [Display(Name = "Κίνηση Προμηθευτή")]
        public int? TransSupplierDefId { get; set; }
        public TransSupplierDef TransSupplierDef { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}