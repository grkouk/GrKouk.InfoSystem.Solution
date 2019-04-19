using System;
using System.Collections.Generic;
using System.Text;
using GrKouk.InfoSystem.Domain.Shared;

namespace GrKouk.InfoSystem.Domain.FinConfig
{
    /// <summary>
    /// Παράμετροι ανα εταιρεία
    /// </summary>
    public class GlobalSettings
    {
        public int Id { get; set; }
        /// <summary>
        /// Default product production series Ids
        /// </summary>
        public int ProductProduceSeriesId { get; set; }
        public int RawMaterialConsumeSeriesId { get; set; }
        //-------------------------------------------------------
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
