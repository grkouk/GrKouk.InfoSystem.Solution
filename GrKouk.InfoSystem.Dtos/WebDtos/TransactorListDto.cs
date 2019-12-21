using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GrKouk.InfoSystem.Domain.Shared;

namespace GrKouk.InfoSystem.Dtos.WebDtos
{
    public class TransactorListDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        
        public string Name { get; set; }
        
        public string EMail { get; set; }
        [Display(Name = "Transactor Type")]
        public string TransactorTypeName { get; set; }

        public string TransactorTypeCode { get; set; }
        public string CompanyCode { get; set; }
       
    }
}
