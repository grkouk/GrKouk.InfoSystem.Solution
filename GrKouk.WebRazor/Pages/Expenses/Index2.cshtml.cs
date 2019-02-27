using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GrKouk.InfoSystem.Dtos;
using GrKouk.WebRazor.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GrKouk.WebRazor.Pages.Expenses
{
    public class Index2Model : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IMapper _mapper;


        public Index2Model(GrKouk.WebApi.Data.ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }
        public string NameSort { get; set; }
        public string NameSortIcon { get; set; }
        public string DateSort { get; set; }
        public string DateSortIcon { get; set; }

        public string CurrentFilter { get; set; }
        public string CurrentDatePeriod { get; set; }
        public string CurrentSort { get; set; }

        public int PageSize { get; set; }
        public int CurrentPageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
       

        public PagedList<FinDiaryExpenseTransactionDto> ListItems { get; set; }

        public async Task OnGetAsync(string sortOrder, string searchString, string datePeriodFilter, int? pageIndex, int? pageSize)
        {
            LoadFilters();

            #region CommentOut

            //PageSize = (int)((pageSize == null || pageSize == 0) ? 20 : pageSize);
            //CurrentPageSize = PageSize;
            //CurrentSort = sortOrder;
            //NameSort = sortOrder == "Name" ? "name_desc" : "Name";
            //DateSort = sortOrder == "Date" ? "date_desc" : "Date";

            //if (searchString != null)
            //{
            //    pageIndex = 1;
            //}
            //else
            //{
            //    searchString = CurrentFilter;
            //}
            //CurrentFilter = searchString;
            
            //CurrentDatePeriod = datePeriodFilter;

            //IQueryable<FinDiaryTransaction> expensesIq = from s in _context.FinDiaryTransactions
            //                                            select s;
            //DateTime fromDate= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            //DateTime toDate = DateTime.Now;
            //switch (datePeriodFilter)
            //{
            //    case "CURMONTH":
            //        fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            //        toDate = DateTime.Now;
            //        break;
            //    case "30DAYS":
            //        toDate = DateTime.Now;
            //        fromDate = toDate.AddDays(-30);
            //        break;
            //    case "60DAYS":
            //        toDate = DateTime.Now;
            //        fromDate = toDate.AddDays(-60);
            //        break;
            //    case "360DAYS":
            //        toDate = DateTime.Now;
            //        fromDate = toDate.AddDays(-360);
            //        break;
            //    case "CURYEAR":
            //        break;
            //    default:
            //        fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            //        toDate = DateTime.Now;
            //        CurrentDatePeriod = "CURMONTH";
            //        break;

            //}

            //expensesIq = expensesIq.Where(p => p.TransactionDate >= fromDate && p.TransactionDate <= toDate);
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    expensesIq = expensesIq.Where(s => s.Transactor.Name.Contains(searchString));
            //}


            //switch (sortOrder)
            //{
            //    case "Date":
            //        expensesIq = expensesIq.OrderBy(p => p.TransactionDate);
            //        DateSortIcon = "fas fa-sort-numeric-up ";
            //        NameSortIcon = "invisible";
            //        break;
            //    case "date_desc":
            //        expensesIq = expensesIq.OrderByDescending(p => p.TransactionDate);
            //        DateSortIcon = "fas fa-sort-numeric-down ";
            //        NameSortIcon = "invisible";
            //        break;
            //    case "Name":
            //        expensesIq = expensesIq.OrderBy(p => p.Transactor.Name);
            //        NameSortIcon = "fas fa-sort-alpha-up ";
            //        DateSortIcon = "invisible";
            //        break;
            //    case "name_desc":
            //        expensesIq = expensesIq.OrderByDescending(p => p.Transactor.Name);
            //        NameSortIcon = "fas fa-sort-alpha-down ";
            //        DateSortIcon = "invisible";
            //        break;
            //    default:
            //        expensesIq = expensesIq.OrderByDescending(p => p.TransactionDate);
            //        break;
            //}



            //expensesIq = expensesIq.Include(f => f.Company)
            //    .Include(f => f.CostCentre)
            //    .Include(f => f.FinTransCategory)
            //    .Include(f => f.Transactor);

            //var t = expensesIq.ProjectTo<FinDiaryExpenseTransactionDto>(_mapper.ConfigurationProvider);


            //ListItems = await PagedList<FinDiaryExpenseTransactionDto>.CreateAsync(
            //    t, pageIndex ?? 1, PageSize);
            //TotalPages = ListItems.TotalPages;
            //TotalCount = ListItems.TotalCount;

            #endregion
        }

        private void LoadFilters()
        {
            var datePeriods = DateFilter.GetDateFiltersSelectList();
            var datePeriodsJs = DateFilter.GetDateFiltersSelectList();

            ViewData["DataFilterValues"] = new SelectList(datePeriods, "Value", "Text");
            ViewData["DataFilterValuesJs"] = datePeriodsJs;



        }
    }
}
