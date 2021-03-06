﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GrKouk.InfoSystem.Dtos.WebDtos.TransactorTransactions;
using GrKouk.WebRazor.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GrKouk.WebRazor.Pages.Transactions.TransactorTransMng
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IMapper _mapper;
        
        public IndexModel(GrKouk.WebApi.Data.ApiDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #region Fields
        public string SectionCode { get; set; } = "SYS-TRANSACTOR-TRANS";
        #endregion
        public decimal sumCredit = 0;
        public decimal sumDebit = 0;
        public PagedList<TransactorTransListDto> ListItems { get; set; }
        public void OnGet()
        {
            LoadFilters();
        }
        private void LoadFilters()
        {
            var datePeriods = DateFilter.GetDateFiltersSelectList();
           
            ViewData["DataFilterValues"] = new SelectList(datePeriods, "Value", "Text");
            var pageFilterSize = PageFilter.GetPageSizeFiltersSelectList();
            ViewData["PageFilterSize"] = new SelectList(pageFilterSize, "Value", "Text");
           
            var companiesList = FiltersHelper.GetCompaniesFilterList(_context);
            ViewData["CompanyFilter"] = new SelectList(companiesList, "Value", "Text");
            ViewData["CurrencySelector"] = new SelectList(FiltersHelper.GetCurrenciesFilterList(_context), "Value", "Text");
            var currencyListJs = _context.Currencies.OrderBy(p => p.Id).AsNoTracking().ToList();
            ViewData["CurrencyListJs"] = currencyListJs;
        }
    }
}
