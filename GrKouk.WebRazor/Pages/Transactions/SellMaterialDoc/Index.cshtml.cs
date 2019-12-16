using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos.WebDtos.SellDocuments;
using GrKouk.WebRazor.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace GrKouk.WebRazor.Pages.Transactions.SellMaterialDoc
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;
       
        #region Fields
        public  string SectionCode { get; set; } = "SYS-SELL-COMBINED-SCN";
        #endregion
        public IndexModel(GrKouk.WebApi.Data.ApiDbContext context,IMapper mapper, IToastNotification toastNotification)
        {
            _context = context;
            _mapper = mapper;
            _toastNotification = toastNotification;
        }
       
        public PagedList<SellDocListDto> ListItems { get; set; }
        public void OnGet()
        {
            LoadFilters();

        }
        private void LoadFilters()
        {
            var datePeriods = DateFilter.GetDateFiltersSelectList();
            // var datePeriodsJs = DateFilter.GetDateFiltersSelectList();

            ViewData["DataFilterValues"] = new SelectList(datePeriods, "Value", "Text");
            var pageFilterSize = PageFilter.GetPageSizeFiltersSelectList();
            ViewData["PageFilterSize"] = new SelectList(pageFilterSize, "Value", "Text");
           
            var companiesList = FiltersHelper.GetCompaniesFilterList(_context);
            ViewData["CompanyFilter"] = new SelectList(companiesList, "Value", "Text");
            ViewData["CurrencySelector"] = new SelectList(FiltersHelper.GetCurrenciesFilterList(_context), "Value", "Text");
        }
    }
}
