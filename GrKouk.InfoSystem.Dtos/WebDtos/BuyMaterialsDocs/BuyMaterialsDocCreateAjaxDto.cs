using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GrKouk.InfoSystem.Domain.Shared;

namespace GrKouk.InfoSystem.Dtos.WebDtos.BuyMaterialsDocs
{
    public class BuyMaterialsDocCreateAjaxDto
    {
        private IList<BuyMaterialDocLineAjaxDto> _buyDocLines;

        public int Id { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime TransDate { get; set; }

        public string TransRefCode { get; set; }
        [Required]
        public int SupplierId { get; set; }
        [Required]
        public int MaterialDocSeriesId { get; set; }
        public decimal AmountFpa { get; set; }
        public decimal AmountNet { get; set; }
        public decimal AmountDiscount { get; set; }
        [MaxLength(500)]
        public string Etiology { get; set; }
        [Required]
        public int CompanyId { get; set; }

        //public Single Q1 { get; set; }
        //public Single Q2 { get; set; }

        public virtual IList<BuyMaterialDocLineAjaxDto> BuyDocLines
        {
            get { return _buyDocLines ?? (_buyDocLines = new List<BuyMaterialDocLineAjaxDto>()); }
            set { _buyDocLines = value; }
        }
    }
}