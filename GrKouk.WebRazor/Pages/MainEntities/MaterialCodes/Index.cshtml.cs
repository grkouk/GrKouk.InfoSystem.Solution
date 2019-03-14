using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using GrKouk.InfoSystem.Definitions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos.WebDtos.WarehouseItems;
using GrKouk.WebApi.Data;
using GrKouk.WebRazor.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GrKouk.WebRazor.Pages.MainEntities.MaterialCodes
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
       
        public IndexModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }
        public string NameSort { get; set; }
        public string NameSortIcon { get; set; }
        public string DateSort { get; set; }
        public string DateSortIcon { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public int PageSizeKartela { get; set; }
        public string TransactorName { get; set; }
        public int TransactorId { get; set; }

        public int ParentPageSize { get; set; }
        public int TransactorTypeFilter { get; set; }
        public int ParentPageIndex { get; set; }
        public bool FiltersVisible { get; set; } = false;
        public bool RowSelectorsVisible { get; set; } = false;
        public string SessionId { get; set; }
        public PagedList<WarehouseItemCode> ListItems { get; set; }

        public async Task OnGetAsync(string sortOrder, string searchString, int transactorId, int? pageIndexKartela
            , int? pageSizeKartela, string transactorName, bool filtersVisible, bool rowSelectorsVisible,string sessionId
            , int? transactorTypeFilter, int? parentPageIndex, int? parentPageSize)
        {
            LoadFilters();
            FiltersVisible = filtersVisible;
            SessionId = sessionId;
            RowSelectorsVisible = rowSelectorsVisible;
            ParentPageSize = (int)(parentPageSize ?? 0);
            ParentPageIndex = (int)(parentPageIndex ?? 0);
            PageSizeKartela = (int)((pageSizeKartela == null || pageSizeKartela == 0) ? 20 : pageSizeKartela);
           // CurrentPageSize = PageSizeKartela;
            CurrentSort = sortOrder;
            NameSort = sortOrder == "Name" ? "name_desc" : "Name";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageIndexKartela = 1;
            }
            else
            {
                searchString = CurrentFilter;
            }
            CurrentFilter = searchString;

            //IQueryable<WarehouseItemCode> fullListIq = from s in _context.WarehouseItemsCodes
            //                                       select s;
            IQueryable<WarehouseItemCode> fullListIq = _context.WarehouseItemsCodes
                .Include(p=>p.WarehouseItem);



            if (!String.IsNullOrEmpty(searchString))
            {
                fullListIq = fullListIq.Where(s => s.WarehouseItem.Name.Contains(searchString));
            }

            switch (sortOrder)
            {

                case "Name":
                    fullListIq = fullListIq.OrderBy(p => p.WarehouseItem.Name);
                    NameSortIcon = "fas fa-sort-alpha-up ";
                    DateSortIcon = "invisible";
                    break;
                case "name_desc":
                    fullListIq = fullListIq.OrderByDescending(p => p.WarehouseItem.Name);
                    NameSortIcon = "fas fa-sort-alpha-down ";
                    DateSortIcon = "invisible";
                    break;
                default:
                    fullListIq = fullListIq.OrderBy(p => p.WarehouseItemId);
                    break;
            }
           // var t = fullListIq.ProjectTo<WarehouseItemListDto>(_mapper.ConfigurationProvider);
            ListItems = await PagedList<WarehouseItemCode>.CreateAsync(fullListIq, pageIndexKartela ?? 1, PageSizeKartela);
        }

        private void LoadFilters()
        {
            var pageFilterSize = FiltersHelper.GetPageSizeFiltersSelectList();
            ViewData["PageFilterSize"] = new SelectList(pageFilterSize, "Value", "Text");
        }
    }
}
