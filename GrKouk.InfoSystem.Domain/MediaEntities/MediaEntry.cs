using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GrKouk.InfoSystem.Domain.MediaEntities
{
    public class MediaEntry
    {
        public int Id { get; set; }
        [MaxLength(250)]
        public string MediaFile { get; set; }
    }
}
