﻿using System.ComponentModel.DataAnnotations;
using GrKouk.InfoSystem.Domain.FinConfig;

namespace GrKouk.InfoSystem.Dtos.WebDtos.Materials
{
    public class MaterialModifyDto
    {
        public int Id { get; set; }

        [MaxLength(20)]
        public string Code { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string ShortDescription { get; set; }
        public string Description { get; set; }

        [Display(Name = "Ενεργό")]
        public bool Active { get; set; }

        [Display(Name = "Βασική ΜΜ")]
        public int? MainMeasureUnitId { get; set; }


        [Display(Name = "Δευ/σα ΜΜ")]
        public int? SecondaryMeasureUnitId { get; set; }

        [Display(Name = "Συν.Mετ.", Prompt = "Συντελεστής μετατροπής σε βασική")]
        public double SecondaryUnitToMainRate { get; set; }

        [Display(Name = "ΜΜ Αγορών")]
        public int? BuyMeasureUnitId { get; set; }

        [Display(Name = "Συν.Mετ.", Prompt = "Συντελεστής μετατροπής σε βασική")]

        public double BuyUnitToMainRate { get; set; }

        [Display(Name = "ΦΠΑ")]
        public int FpaDefId { get; set; }

        [MaxLength(50)]
        public string BarCode { get; set; }
        [MaxLength(50)]
        public string ManufacturerCode { get; set; }

        [Display(Name = "Κατ.Είδους")]
        public int MaterialCategoryId { get; set; }
        /// <summary>
        /// Τύπος υλικού, Σετ, Κανονικό, Συντιθέμενο
        /// </summary>
        [Display(Name = "Τύπος Είδους", Prompt = "Τύπος υλικού, Σετ, Κανονικό, Συντιθέμενο")]

        public MaterialTypeEnum MaterialType { get; set; }
        [Display(Name = "Φύση Είδους", Prompt = "Υλικό,Υπηρεσία,Παγιο,Δαπάνη")]
        public MaterialNatureEnum MaterialNature { get; set; }
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

    }
}