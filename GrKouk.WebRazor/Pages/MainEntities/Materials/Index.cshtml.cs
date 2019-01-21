using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos.WebDtos.Materials;
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

        public PagedList<MaterialListDto> ListItems { get; set; }
       // public IList<Material> Material { get;set; }

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

            IQueryable<Material> fullListIq = from s in _context.Materials
                                                         select s;


            MaterialNatureEnum natureFilterValue = MaterialNatureEnum.MaterialNatureEnumUndefined;

            switch (CurrentMaterialNature)
            {
                case "MaterialNatureEnumUndefined":
                    natureFilterValue = MaterialNatureEnum.MaterialNatureEnumUndefined;
                    break;
                case "MaterialNatureEnumMaterial":
                    natureFilterValue = MaterialNatureEnum.MaterialNatureEnumMaterial;
                    break;
                case "MaterialNatureEnumService":
                    natureFilterValue = MaterialNatureEnum.MaterialNatureEnumService;
                    break;
                case "MaterialNatureEnumExpense":
                    natureFilterValue = MaterialNatureEnum.MaterialNatureEnumExpense;
                    break;
                case "MaterialNatureEnumFixedAsset":
                    natureFilterValue = MaterialNatureEnum.MaterialNatureEnumFixedAsset;
                    break;
                default:
                    natureFilterValue = MaterialNatureEnum.MaterialNatureEnumUndefined;
                    break;
            }

            if (natureFilterValue!=MaterialNatureEnum.MaterialNatureEnumUndefined)
            {
                fullListIq = fullListIq.Where(s => s.MaterialNature==natureFilterValue);
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
            var t = fullListIq.ProjectTo<MaterialListDto>(_mapper.ConfigurationProvider);
            ListItems = await PagedList<MaterialListDto>.CreateAsync(
                t, pageIndex ?? 1, PageSize);
        }
        private void LoadFilters()
        {
            List<SelectListItem> materialNatures = new List<SelectListItem>
            {
                new SelectListItem() {Value = "0", Text = "{All Natures}"},
                new SelectListItem() {Value = MaterialNatureEnum.MaterialNatureEnumMaterial.ToString(), Text = "Υλικό"},
                new SelectListItem() {Value = MaterialNatureEnum.MaterialNatureEnumService.ToString(), Text = "Υπηρεσία"},
                new SelectListItem() {Value = MaterialNatureEnum.MaterialNatureEnumExpense.ToString(), Text = "Δαπάνη"},
                new SelectListItem() {Value = MaterialNatureEnum.MaterialNatureEnumFixedAsset.ToString(), Text = "Πάγιο"}
                

            };
            ViewData["MaterialNatureValues"] = new SelectList(materialNatures, "Value", "Text");
        }
    }
}
