using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos;
using GrKouk.WebApi.Data;
using GrKouk.WebRazor.Helpers;

namespace GrKouk.WebRazor.Pages.Expenses
{
    public class IndexModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IMapper _mapper;
        

        public IndexModel(GrKouk.WebApi.Data.ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            
        }
        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public int PageSize { get; set; }
        public int CurrentPageSize { get; set; }

        public PaginatedList<FinDiaryExpenseTransactionDto> FinDiaryTransaction { get; set; }

        public async Task OnGetAsync(string sortOrder, string searchString, int? pageIndex,int? pageSize)
        {
            PageSize = (int)((pageSize == null || pageSize == 0) ? 20 : pageSize);
            CurrentPageSize = PageSize;

            CurrentSort = sortOrder;
            
            DateSort = String.IsNullOrEmpty( sortOrder)  ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = CurrentFilter;
            }
            CurrentFilter = searchString;

            IQueryable<FinDiaryTransaction> expensesIq = from s in _context.FinDiaryTransactions
                select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                expensesIq = expensesIq.Where(s => s.Transactor.Name.Contains(searchString));
            }
            //IQueryable<FinDiaryTransaction> apiDbContext =  _context.FinDiaryTransactions
            //    .Include(f => f.Company)
            //    .Include(f => f.CostCentre)
            //    .Include(f => f.FinTransCategory)
            //    .Include(f => f.Transactor);

            switch (sortOrder)
            {
                case "Date":
                    expensesIq = expensesIq.OrderBy(p => p.TransactionDate);
                    //apiDbContext = apiDbContext.OrderBy(p => p.TransactionDate);
                    break;
                case "date_desc":
                    expensesIq= expensesIq.OrderByDescending(p => p.TransactionDate);
                    //apiDbContext=apiDbContext.OrderByDescending(p => p.TransactionDate);
                    break;
                default:
                    expensesIq = expensesIq.OrderByDescending(p => p.TransactionDate);
                    //apiDbContext= apiDbContext.OrderByDescending(p => p.TransactionDate);
                    break;
            }

            

            expensesIq=expensesIq.Include(f => f.Company)
                .Include(f => f.CostCentre)
                .Include(f => f.FinTransCategory)
                .Include(f => f.Transactor);
            
            var t = expensesIq.ProjectTo<FinDiaryExpenseTransactionDto>(_mapper.ConfigurationProvider);

           
            FinDiaryTransaction = await PaginatedList<FinDiaryExpenseTransactionDto>.CreateAsync( 
                t, pageIndex ?? 1, PageSize);
          
        }
    }
}
