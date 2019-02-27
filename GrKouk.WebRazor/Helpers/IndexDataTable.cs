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

    public class IndexDataTableResponse
    {
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
        
        public List<FinDiaryExpenseTransactionDto> Data { get; set; }
        public decimal SumOfAmount { get; set; }

    }
}
