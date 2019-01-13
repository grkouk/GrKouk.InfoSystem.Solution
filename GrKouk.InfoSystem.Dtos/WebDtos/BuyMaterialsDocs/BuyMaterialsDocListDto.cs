using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GrKouk.InfoSystem.Domain.Shared;

namespace GrKouk.InfoSystem.Dtos.WebDtos.BuyMaterialsDocs
{
    public class BuyMaterialsDocListDto
    {
        //private ICollection<BuyMaterialsDocLine> _buyDocLines;

        public int Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:ddd dd MMM yyyy}")]
        [Display(Name = "Date")]
        public DateTime TransDate { get; set; }
        [Display(Name = "Ref.")]
        public string TransRefCode { get; set; }

        public int SectionId { get; set; }
        [Display(Name = "Section")]
        public string SectionCode { get; set; }

        public int SupplierId { get; set; }
        [Display(Name = "Transactor")]
        public string SupplierName { get; set; }

        public int MaterialDocSeriesId { get; set; }
        [Display(Name = "Series")]
        public string MaterialDocSeriesCode { get; set; }
        [Display(Name = "Series")]
        public string MaterialDocSeriesName { get; set; }

        public decimal AmountFpa { get; set; }
        public decimal AmountNet { get; set; }
        public decimal AmountDiscount { get; set; }
        [Display(Name = "Total Amount")]
        public decimal TotalAmount
        {
            get => AmountNet + AmountFpa - AmountDiscount;

        }
        public int CompanyId { get; set; }

        [Display(Name = "Company")]
        public string CompanyCode { get; set; }

    }
}
