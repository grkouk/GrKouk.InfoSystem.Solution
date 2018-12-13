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

        public string TransactorTypeName { get; set; }

        public string TransactorTypeCode { get; set; }

        //[MaxLength(200)]
        //public string Address { get; set; }
        //[MaxLength(50)]
        //public string City { get; set; }
        //public int? Zip { get; set; }

        //[MaxLength(200)]
        //public string PhoneWork { get; set; }
        //[MaxLength(200)]
        //public string PhoneMobile { get; set; }
        //[MaxLength(200)]
        //public string PhoneFax { get; set; }
        //public int TransactorTypeId { get; set; }
        //public TransactorType TransactorType { get; set; }
    }
}
