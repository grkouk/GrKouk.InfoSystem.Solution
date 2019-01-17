using System.ComponentModel.DataAnnotations;
using GrKouk.InfoSystem.Domain.FinConfig;

namespace GrKouk.InfoSystem.Dtos.WebDtos.Materials
{
    public class MaterialSearchListDto
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }

        public MaterialNatureEnum MaterialNature { get; set; }
       
        public string MaterialNatureName
        {
            get
            {
                string ret = "";
                switch (MaterialNature)
                {
                    case MaterialNatureEnum.MaterialNatureEnumUndefined:
                        ret = "{{Απρ}}";
                        break;
                    case MaterialNatureEnum.MaterialNatureEnumMaterial:
                        ret = "{Υλ}";
                        break;
                    case MaterialNatureEnum.MaterialNatureEnumService:
                        ret = "{Υπ}";
                        break;
                    case MaterialNatureEnum.MaterialNatureEnumExpense:
                        ret = "{Δαπ}";
                        break;
                    case MaterialNatureEnum.MaterialNatureEnumFixedAsset:
                        ret = "{Πάγ}";
                        break;
                    default:
                        ret = "{{Απρ}}";
                        break;
                }
                return ret;
            }
        }

        public string Value => this.Id.ToString();

        public string Label
        {
            get
            {
               var r= $"{this.Name} {this.MaterialNatureName}";
               return r;
            }
        }
    }
}