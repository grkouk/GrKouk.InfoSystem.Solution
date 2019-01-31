using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GrKouk.InfoSystem.Dtos.WebDtos.SellDocuments
{
    public class SellDocCreateAjaxDto
    {
        private IList<SellDocLineAjaxDto> _sellDocLines;

        public int Id { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime TransDate { get; set; }

        public string TransRefCode { get; set; }
        [Required]
        public int TransactorId { get; set; }
        [Required]
        public int SellDocSeriesId { get; set; }
        public decimal AmountFpa { get; set; }
        public decimal AmountNet { get; set; }
        public decimal AmountDiscount { get; set; }
        [MaxLength(500)]
        public string Etiology { get; set; }
        [Required]
        public int CompanyId { get; set; }

        //public Single Q1 { get; set; }
        //public Single Q2 { get; set; }

        public virtual IList<SellDocLineAjaxDto> SellDocLines
        {
            get { return _sellDocLines ?? (_sellDocLines = new List<SellDocLineAjaxDto>()); }
            set { _sellDocLines = value; }
        }
    }
}
