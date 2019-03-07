using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.InfoSystem.Dtos;

namespace GrKouk.WebRazor.Helpers
{
    public class IndexDataTableRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string SortData { get; set; }
        public string DateRange { get; set; }
        public string CompanyFilter { get; set; }
        public string MaterialNatureFilter { get; set; }

    }

    public class IndexDataTableResponse<T>
    {
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
        public decimal SumOfAmount { get; set; }
        public decimal SumOfDebit { get; set; }
        public decimal SumOfCredit { get; set; }
        public decimal SumImportVolume { get; set; }
        public decimal SumExportVolume { get; set; }
        public decimal SumImportValue { get; set; }
        public decimal SumExportValue { get; set; }
        public List<T> Data { get; set; }

    }
}
