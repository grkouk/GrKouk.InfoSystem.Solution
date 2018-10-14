﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GrKouk.InfoSystem.Domain.Shared
{
  public  class Transactor
    {
        public int Id { get; set; }

        [MaxLength(15)]
        public string Code { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }
        [MaxLength(50)]
        public string City { get; set; }
        public int? Zip { get; set; }

        [MaxLength(200)]
        public string PhoneWork { get; set; }
        [MaxLength(200)]
        public string PhoneMobile { get; set; }
        [MaxLength(200)]
        public string PhoneFax { get; set; }

        [MaxLength(200)]
        public string EMail { get; set; }

        public int TransactorTypeId { get; set; }
        public TransactorType TransactorType { get; set; }
        
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
