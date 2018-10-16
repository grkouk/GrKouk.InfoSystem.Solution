using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GrKouk.InfoSystem.Domain.FinConfig
{
    public class FpaDef
    {
        public int Id { get; set; }

        [MaxLength(15)] [Required] public string Code { get; set; }

        [MaxLength(200)] [Required] public string Name { get; set; }

        public Single Rate { get; set; }

    }
}