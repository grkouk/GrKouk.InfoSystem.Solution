using GrKouk.InfoSystem.Domain.Shared;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GrKouk.InfoSystem.Domain.FinConfig
{
    public class BuyMaterialDocSeriesDef
    {
        public int Id { get; set; }

        [MaxLength(15)]
        [Required]
        public string Code { get; set; }

        [Display(Name = "Συντόμευση", Prompt = "Συντόμευση της σειράς")]
        [MaxLength(20)]
        public string Abbr { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        [Display(Name = "Ενεργό")]
        [DefaultValue(true)]
        public bool Active { get; set; }

        [Display(Name = "Τύπος Παραστατικού")]
        public int BuyMaterialDocTypeDefId { get; set; }
        public virtual BuyMaterialDocTypeDef BuyMaterialDocTypeDef { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

    }
}
