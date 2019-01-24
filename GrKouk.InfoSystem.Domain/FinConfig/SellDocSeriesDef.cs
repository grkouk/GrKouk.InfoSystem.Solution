using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GrKouk.InfoSystem.Domain.Shared;

namespace GrKouk.InfoSystem.Domain.FinConfig
{
    public class SellDocSeriesDef
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
        public int SellDocTypeDefId { get; set; }
        public virtual SellDocTypeDef SellDocTypeDef { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

    }
}