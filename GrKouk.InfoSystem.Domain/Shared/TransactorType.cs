using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GrKouk.InfoSystem.Domain.Shared
{
    /// <summary>
    /// Transactor types
    /// 1->Customer
    /// 2->Supplier
    /// 3->
    /// </summary>
  public  class TransactorType
    {
        public int Id { get; set; }

        [Required]
        public Boolean IsSystem { get; set; }

        [MaxLength(15)]
        [Required]
        public string Code { get; set; }

        [MaxLength(200)]
        [Required]
        public string Name { get; set; }
    }
}
