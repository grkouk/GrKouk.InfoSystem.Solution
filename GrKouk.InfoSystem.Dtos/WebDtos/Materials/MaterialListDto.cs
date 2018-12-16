using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
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
        [Display(Name = "Κατ.Υλικού")]
        public string MaterialCateroryName { get; set; }

        [Display(Name = "Τύπος Υλικού")]
        public MaterialTypeEnum MaterialType { get; set; }

        public int CompanyId { get; set; }
        public string CompanyCode { get; set; }
    }
}
