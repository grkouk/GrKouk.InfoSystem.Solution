﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos.WebDtos.BuyMaterialsDocs;
using GrKouk.WebRazor.Helpers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;

namespace GrKouk.WebRazor.Pages.Transactions.SellMaterialDoc
{
    public class IndexModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;

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
        public IndexModel(GrKouk.WebApi.Data.ApiDbContext context,IMapper mapper, IToastNotification toastNotification)
        {
            _context = context;
            _mapper = mapper;
            _toastNotification = toastNotification;
        }

       
        public PagedList<BuyMaterialsDocListDto> ListItems { get; set; }
        public async Task OnGetAsync(string sortOrder, string searchString, string datePeriodFilter, int? pageIndex, int? pageSize)
        {
            LoadFilters();
            PageSize = (int)((pageSize == null || pageSize == 0) ? 20 : pageSize);
            CurrentPageSize = PageSize;
            CurrentSort = sortOrder;
            NameSort = sortOrder == "Name" ? "name_desc" : "Name";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = CurrentFilter;
            }
            CurrentFilter = searchString;
            CurrentDatePeriod = datePeriodFilter;
            IQueryable<BuyMaterialsDocument> fullListIq = _context.BuyMaterialsDocuments;
            DateTime fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime toDate = DateTime.Now;
            switch (datePeriodFilter)
            {
                case "CURMONTH":
                    fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    toDate = DateTime.Now;
                    break;
                case "30DAYS":
                    toDate = DateTime.Now;
                    fromDate = toDate.AddDays(-30);
                    break;
                case "60DAYS":
                    toDate = DateTime.Now;
                    fromDate = toDate.AddDays(-60);
                    break;
                case "360DAYS":
                    toDate = DateTime.Now;
                    fromDate = toDate.AddDays(-360);
                    break;
                case "CURYEAR":
                    break;
                default:
                    fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    toDate = DateTime.Now;
                    CurrentDatePeriod = "CURMONTH";
                    break;

            }
            fullListIq = fullListIq.Where(p => p.TransDate >= fromDate && p.TransDate <= toDate);
            if (!String.IsNullOrEmpty(searchString))
            {
                fullListIq = fullListIq.Where(s => s.Supplier.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "Date":
                    fullListIq = fullListIq.OrderBy(p => p.TransDate);
                    DateSortIcon = "fas fa-sort-numeric-up ";
                    NameSortIcon = "invisible";
                    break;
                case "date_desc":
                    fullListIq = fullListIq.OrderByDescending(p => p.TransDate);
                    DateSortIcon = "fas fa-sort-numeric-down ";
                    NameSortIcon = "invisible";
                    break;
                case "Name":
                    fullListIq = fullListIq.OrderBy(p => p.Supplier.Name);
                    NameSortIcon = "fas fa-sort-alpha-up ";
                    DateSortIcon = "invisible";
                    break;
                case "name_desc":
                    fullListIq = fullListIq.OrderByDescending(p => p.Supplier.Name);
                    NameSortIcon = "fas fa-sort-alpha-down ";
                    DateSortIcon = "invisible";
                    break;
                default:
                    fullListIq = fullListIq.OrderBy(p => p.Id);
                    break;
            }

            var t = fullListIq.ProjectTo<BuyMaterialsDocListDto>(_mapper.ConfigurationProvider);

            ListItems = await PagedList<BuyMaterialsDocListDto>.CreateAsync(t, pageIndex ?? 1, PageSize);
           
        }
        private void LoadFilters()
        {
            List<SelectListItem> datePeriods = new List<SelectListItem>
            {
                new SelectListItem() {Value = "CURMONTH", Text = "Τρέχων Μήνας"},
                new SelectListItem() {Value = "30DAYS", Text = "30 Ημέρες"},
                new SelectListItem() {Value = "60DAYS", Text = "60 Ημέρες"},
                new SelectListItem() {Value = "360DAYS", Text = "360 Ημέρες"},
                new SelectListItem() {Value = "CURYEAR", Text = "Τρέχων Ετος"}

            };



            ViewData["DataFilterValues"] = new SelectList(datePeriods, "Value", "Text");
        }
    }
}
