using System.ComponentModel.DataAnnotations;
using GrKouk.InfoSystem.Domain.Shared;

namespace GrKouk.InfoSystem.Domain.FinConfig
{
    /// <summary>
    /// Σειρά παραστατικού κίνησης προμηθευτή
    /// </summary>
    public class TransSupplierDocSeriesDef
    {
        public int Id { get; set; }

        [MaxLength(15)]
        public string Code { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }
        [Display(Name = "Ενεργό")]
        public bool Active { get; set; }

        [Display(Name = "Τύπος Παραστατικού")]
        public int TransSupplierDocTypeDefId { get; set; }
        public TransSupplierDocTypeDef TransSupplierDocTypeDef { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}