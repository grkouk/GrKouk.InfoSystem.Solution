using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GrKouk.InfoSystem.Domain.FinConfig;

namespace GrKouk.InfoSystem.Domain.Shared
{
    public class DiaryDef
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DiaryTypeEnum DiaryType { get; set; }
        //[MaxLength(200)]
        //public string Definition { get; set; }
        [MaxLength(200)]
        public string SelectedDocTypes { get; set; }
    }
}
