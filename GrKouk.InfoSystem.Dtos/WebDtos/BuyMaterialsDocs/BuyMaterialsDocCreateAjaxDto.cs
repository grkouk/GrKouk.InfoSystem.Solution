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

        [DataType(DataType.Date)]
        public DateTime TransDate { get; set; }

        public string TransRefCode { get; set; }

        public virtual IList<BuyMaterialDocLineAjaxDto> BuyDocLines
        {
            get { return _buyDocLines ?? (_buyDocLines = new List<BuyMaterialDocLineAjaxDto>()); }
            set { _buyDocLines = value; }
        }
    }
}