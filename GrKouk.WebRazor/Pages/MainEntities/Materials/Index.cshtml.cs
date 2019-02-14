using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GrKouk.InfoSystem.Definitions;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos.WebDtos.WarehouseItems;
using GrKouk.WebRazor.Helpers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GrKouk.WebRazor.Pages.MainEntities.Materials
{
    public class IndexModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IMapper _mapper;

        public string NameSort { get; set; }
        public string NameSortIcon { get; set; }
        public string DateSort { get; set; }
        public string DateSortIcon { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentMaterialNature { get; set; }
        public string CurrentSort { get; set; }
        public int PageSize { get; set; }
        public int CurrentPageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public IndexModel(GrKouk.WebApi.Data.ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public PagedList<WarehouseItemListDto> ListItems { get; set; }
       // public IList<WarehouseItem> WarehouseItem { get;set; }

        public async Task OnGetAsync(string sortOrder, string searchString, string materialNatureFilter, int? pageIndex, int? pageSize)
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
            CurrentMaterialNature = materialNatureFilter;

            IQueryable<WarehouseItem> fullListIq = from s in _context.WarehouseItems
                                                         select s;


            WarehouseItemNatureEnum natureFilterValue = WarehouseItemNatureEnum.WarehouseItemNatureUndefined;

            switch (CurrentMaterialNature)
            {
                case "WarehouseItemNatureUndefined":
                    natureFilterValue = WarehouseItemNatureEnum.WarehouseItemNatureUndefined;
                    break;
                case "WarehouseItemNatureMaterial":
                    natureFilterValue = WarehouseItemNatureEnum.WarehouseItemNatureMaterial;
                    break;
                case "WarehouseItemNatureService":
                    natureFilterValue = WarehouseItemNatureEnum.WarehouseItemNatureService;
                    break;
                case "WarehouseItemNatureExpense":
                    natureFilterValue = WarehouseItemNatureEnum.WarehouseItemNatureExpense;
                    break;
                case "WarehouseItemNatureFixedAsset":
                    natureFilterValue = WarehouseItemNatureEnum.WarehouseItemNatureFixedAsset;
                    break;
                default:
                    natureFilterValue = WarehouseItemNatureEnum.WarehouseItemNatureUndefined;
                    break;
            }

            if (natureFilterValue!=WarehouseItemNatureEnum.WarehouseItemNatureUndefined)
            {
                fullListIq = fullListIq.Where(s => s.WarehouseItemNature==natureFilterValue);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                fullListIq = fullListIq.Where(s => s.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
               
                case "Name":
                    fullListIq = fullListIq.OrderBy(p => p.Name);
                    NameSortIcon = "fas fa-sort-alpha-up ";
                    DateSortIcon = "invisible";
                    break;
                case "name_desc":
                    fullListIq = fullListIq.OrderByDescending(p => p.Name);
                    NameSortIcon = "fas fa-sort-alpha-down ";
                    DateSortIcon = "invisible";
                    break;
                default:
                    fullListIq = fullListIq.OrderBy(p => p.Id);
                    break;
            }
            var t = fullListIq.ProjectTo<WarehouseItemListDto>(_mapper.ConfigurationProvider);
            ListItems = await PagedList<WarehouseItemListDto>.CreateAsync(
                t, pageIndex ?? 1, PageSize);
        }
        private void LoadFilters()
        {
            List<SelectListItem> materialNatures = new List<SelectListItem>
            {
                new SelectListItem() {Value = "0", Text = "{All Natures}"},
                new SelectListItem() {Value = WarehouseItemNatureEnum.WarehouseItemNatureMaterial.ToString(), Text = "Υλικό"},
                new SelectListItem() {Value = WarehouseItemNatureEnum.WarehouseItemNatureService.ToString(), Text = "Υπηρεσία"},
                new SelectListItem() {Value = WarehouseItemNatureEnum.WarehouseItemNatureExpense.ToString(), Text = "Δαπάνη"},
                new SelectListItem() {Value = WarehouseItemNatureEnum.WarehouseItemNatureFixedAsset.ToString(), Text = "Πάγιο"}
                

            };
            ViewData["MaterialNatureValues"] = new SelectList(materialNatures, "Value", "Text");
        }
    }
}
