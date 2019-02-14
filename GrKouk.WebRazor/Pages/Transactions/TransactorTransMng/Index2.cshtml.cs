﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos.WebDtos.TransactorTransactions;
using GrKouk.WebApi.Data;
using GrKouk.WebRazor.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GrKouk.WebRazor.Pages.Transactions.TransactorTransMng
{
    public class Index2Model : PageModel
    {
        private readonly ApiDbContext _context;
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
        public int CompanyFilter { get; set; }
        public bool FiltersVisible { get; set; } = false;
        public bool RowSelectorsVisible { get; set; } = false;
        public PagedList<TransactorTransListDto> ListItems { get; set; }
        public decimal sumCredit = 0;
        public decimal sumDebit = 0;
        public string FiltersShow { get; set; }
        public string FiltersShowText { get; set; }
        public string RowSelectorsHidden { get; set; }
        public string SelectedToggleText { get; set; }
        public string SelectedRowsMenuItem { get; set; }
        public async Task OnGetAsync(string sortOrder, string searchString, string datePeriodFilter, int? companyFilter
            , bool filtersVisible, bool rowSelectorsVisible, int? pageIndex, int? pageSize)
        {
            LoadFilters();
            FiltersVisible = filtersVisible;
            RowSelectorsVisible = rowSelectorsVisible;
            PageSize = (int)((pageSize == null || pageSize == 0) ? 20 : pageSize);
            CompanyFilter = (int)(companyFilter ?? 0);
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
            IQueryable<TransactorTransaction> fullListIq = _context.TransactorTransactions;
            if (companyFilter > 0)
            {
                fullListIq = fullListIq.Where(p => p.CompanyId == companyFilter);
            }
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
                fullListIq = fullListIq.Where(s => s.Transactor.Name.Contains(searchString));
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
                    fullListIq = fullListIq.OrderBy(p => p.Transactor.Name);
                    NameSortIcon = "fas fa-sort-alpha-up ";
                    DateSortIcon = "invisible";
                    break;
                case "name_desc":
                    fullListIq = fullListIq.OrderByDescending(p => p.Transactor.Name);
                    NameSortIcon = "fas fa-sort-alpha-down ";
                    DateSortIcon = "invisible";
                    break;
                default:
                    fullListIq = fullListIq.OrderBy(p => p.Id);
                    break;
            }
            var t = fullListIq.ProjectTo<TransactorTransListDto>(_mapper.ConfigurationProvider);
            ListItems = await PagedList<TransactorTransListDto>.CreateAsync(
                t, pageIndex ?? 1, PageSize);
            sumDebit = ListItems.Sum(p => p.DebitAmount);
            sumCredit = ListItems.Sum(p => p.CreditAmount);

             FiltersShow = FiltersVisible ? "show" : "";
             FiltersShowText = FiltersVisible ? "Hide Filters" : "Show Filters";
            RowSelectorsHidden = RowSelectorsVisible ? "" : "hidden";
            SelectedToggleText = RowSelectorsVisible ? "Hide Row Selectors" : "Show Row Selectors";
            SelectedRowsMenuItem = RowSelectorsVisible ? "" : "hidden ";


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

            var dbCompanies = _context.Companies.OrderBy(p => p.Code).AsNoTracking();
            List<SelectListItem> companiesList = new List<SelectListItem>();
            companiesList.Add(new SelectListItem() { Value = 0.ToString(), Text = "{All Companies}" });
            foreach (var company in dbCompanies)
            {
                companiesList.Add(new SelectListItem() { Value = company.Id.ToString(), Text = company.Code });
            }
            ViewData["CompanyFilter"] = new SelectList(companiesList, "Value", "Text");

            var pageFilterSize = PageFilter.GetPageSizeFiltersSelectList();
            ViewData["PageFilterSize"] = new SelectList(pageFilterSize, "Value", "Text");
        }
    }

    public class PartialIndex
    {
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
        public int CompanyFilter { get; set; }
        public bool FiltersVisible { get; set; } = false;
        public bool RowSelectorsVisible { get; set; } = false;
        public PagedList<TransactorTransListDto> ListItems { get; set; }
        public decimal sumCredit = 0;
        public decimal sumDebit = 0;
    }
}