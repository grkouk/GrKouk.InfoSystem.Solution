using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GrKouk.InfoSystem.Domain.Shared
{
    public class Currency
    {
        public int Id { get; set; }

        [MaxLength(20)]
        [Required]
        public string Code { get; set; }
        [MaxLength(20)]
        public string DisplayLocale { get; set; }
        [MaxLength(200)]
        [Required]
        public string Name { get; set; }
    }
}
