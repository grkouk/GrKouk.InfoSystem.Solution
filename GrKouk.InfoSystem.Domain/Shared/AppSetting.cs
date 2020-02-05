using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GrKouk.InfoSystem.Domain.Shared
{
    public class AppSetting
    {
        [Display(Name = "Setting Code")]
        [MaxLength(20)]
        [Required]
        public string Code { get; set; }
        [Display(Name = "Setting Value")]
        [MaxLength(50)]
        [Required]
        public string Value { get; set; }
    }
}
