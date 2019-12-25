using AutoMapper;
using GrKouk.InfoSystem.Dtos.WebDtos.BuyDocuments;
using GrKouk.InfoSystem.Dtos.WebDtos.WarehouseItems;
using GrKouk.WebRazor.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;

namespace GrKouk.WebRazor.Pages.MainEntities.MaterialCodes
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;
       
        public IndexModel(GrKouk.WebApi.Data.ApiDbContext context, IMapper mapper, IToastNotification toastNotification)
        {
            _context = context;
            _mapper = mapper;
            _toastNotification = toastNotification;
        }


        public PagedList<WrItemCodeListDto> ListItems { get; set; }
        public void OnGet()
        {
            LoadFilters();

        }
        private void LoadFilters()
        {
           
            var pageFilterSize = PageFilter.GetPageSizeFiltersSelectList();
            ViewData["PageFilterSize"] = new SelectList(pageFilterSize, "Value", "Text");

            var companiesList = FiltersHelper.GetCompaniesFilterList(_context);
            ViewData["CompanyFilter"] = new SelectList(companiesList, "Value", "Text");
           
        }
    }
}
