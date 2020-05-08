﻿using System;

namespace GrKouk.WebRazor.Helpers
{
    public class TransactorFinancialDataResponse
    {
        public decimal SumOfDebit { get; set; }
        public decimal SumOfCredit { get; set; }
        public decimal SumOfDifference { get; set; }
        public DateTime LastDebitDate { get; set; }
        public DateTime LastCreditDate { get; set; }
    }
}