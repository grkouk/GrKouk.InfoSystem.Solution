﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GrKouk.InfoSystem.Dtos.WebDtos.SellDocuments
{
    public class SellDocModifyAjaxNoLinesDto
    {
        public int Id { get; set; }
        public DateTime TransDate { get; set; }

        public string TransRefCode { get; set; }

        public int TransactorId { get; set; }

        public int SellDocSeriesId { get; set; }
        public int SellDocTypeId { get; set; }
        public decimal AmountFpa { get; set; }
        public decimal AmountNet { get; set; }
        public decimal AmountDiscount { get; set; }

        public string Etiology { get; set; }
        public int FiscalPeriodId { get; set; }
        public int CompanyId { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
