using System;
using System.Collections.Generic;
using System.Text;

namespace GrKouk.InfoSystem.Domain.FinConfig
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int DaysOverdue { get; set; }

    }

    public class FinancialAccount
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
}
