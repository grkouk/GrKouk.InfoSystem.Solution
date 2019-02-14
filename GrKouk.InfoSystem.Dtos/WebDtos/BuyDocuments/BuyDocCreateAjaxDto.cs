using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GrKouk.InfoSystem.Dtos.WebDtos.BuyDocuments
{
    public class BuyDocCreateAjaxDto
    {
        private IList<BuyDocLineAjaxDto> _buyDocLines;

        public int Id { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime TransDate { get; set; }

        public string TransRefCode { get; set; }
        [Required]
        public int TransactorId { get; set; }
        [Required]
        public int BuyDocSeriesId { get; set; }
        public decimal AmountFpa { get; set; }
        public decimal AmountNet { get; set; }
        public decimal AmountDiscount { get; set; }
        [MaxLength(500)]
        public string Etiology { get; set; }
        [Required]
        public int CompanyId { get; set; }

        //public Single Q1 { get; set; }
        //public Single Q2 { get; set; }

        public virtual IList<BuyDocLineAjaxDto> BuyDocLines
        {
            get { return _buyDocLines ?? (_buyDocLines = new List<BuyDocLineAjaxDto>()); }
            set { _buyDocLines = value; }
        }
    }
}