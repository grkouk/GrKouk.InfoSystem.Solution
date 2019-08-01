using System;
using System.Collections.Generic;
using System.Text;
using GrKouk.InfoSystem.Definitions;

namespace GrKouk.InfoSystem.Domain.FinConfig
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int DaysOverdue { get; set; }
        public SeriesAutoPayoffEnum AutoPayoffWay { get; set; }
        public int? PayoffSeriesId { get; set; }
    }

    public class FinancialAccount
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
}
