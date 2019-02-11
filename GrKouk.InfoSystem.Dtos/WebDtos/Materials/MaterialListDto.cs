using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GrKouk.InfoSystem.Definitions;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.InfoSystem.Domain.Shared;

namespace GrKouk.InfoSystem.Dtos.WebDtos.Materials
{
    public class MaterialListDto
    {
        public int Id { get; set; }


        public string Code { get; set; }


        public string Name { get; set; }


        [Display(Name = "Ενεργό")]
        public bool Active { get; set; }

        [Display(Name = "Βασική ΜΜ")]
        public int? MainMeasureUnitId { get; set; }
        public string MainMeasureUnitCode { get; set; }

        [Display(Name = "ΜΜ Αγορών")]
        public int? BuyMeasureUnitId { get; set; }
        public string BuyMeasureUnitCode { get; set; }

        [Display(Name = "ΦΠΑ")]
        public int FpaDefId { get; set; }
        [Display(Name = "ΦΠΑ")]
        public string FpaDefName { get; set; }

        [MaxLength(50)]
        public string BarCode { get; set; }
        [MaxLength(50)]
        public string ManufacturerCode { get; set; }

        public int MaterialCategoryId { get; set; }
        [Display(Name = "Κατ.Είδους")]
        public string MaterialCateroryName { get; set; }


        public MaterialTypeEnum MaterialType { get; set; }
        [Display(Name = "Τύπος Είδους")]
        public string MaterialTypeName
        {
            get
            {
                string ret = "";
                switch (MaterialType)
                {

                    case MaterialTypeEnum.MaterialTypeNormal:
                        ret = "Κανονικό";
                        break;
                    case MaterialTypeEnum.MaterialTypeSet:
                        ret = "Σετ";
                        break;
                    case MaterialTypeEnum.MaterialTypeComposed:
                        ret = "Συντιθέμενο";
                        break;
                    default:
                        ret = "Απροσδιόριστο";
                       break;
                }
                return ret;
            }
            
        }
        [Display(Name = "Καθ.Τιμή")]
        public decimal PriceNetto { get; set; }
        [Display(Name = "Μικτή Τιμή")]
        public decimal PriceBrutto { get; set; }
        public MaterialNatureEnum MaterialNature { get; set; }
        [Display(Name = "Φύση Είδους", Prompt = "Υλικό,Υπηρεσία,Παγιο,Δαπάνη")]
        public string MaterialNatureName {
            get
            {
                string ret = "";
                switch (MaterialNature)
                {
                    case MaterialNatureEnum.MaterialNatureEnumUndefined:
                        ret = "Απροσδιόριστο";
                        break;
                    case MaterialNatureEnum.MaterialNatureEnumMaterial:
                        ret = "Υλικό";
                        break;
                    case MaterialNatureEnum.MaterialNatureEnumService:
                        ret = "Υπηρεσία";
                        break;
                    case MaterialNatureEnum.MaterialNatureEnumExpense:
                        ret = "Δαπάνη";
                        break;
                    case MaterialNatureEnum.MaterialNatureEnumFixedAsset:
                        ret = "Πάγιο";
                        break;
                    default:
                        ret = "Απροσδιόριστο";
                        break;
                }
                return ret;
            }
        }
        public int CompanyId { get; set; }
        public string CompanyCode { get; set; }
    }
}
