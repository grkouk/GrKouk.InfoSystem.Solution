using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GrKouk.InfoSystem.Dtos.WebDtos.WarehouseTransactions;
using GrKouk.WebRazor.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GrKouk.WebRazor.Pages.MainEntities.Materials
{
    [Authorize(Roles = "Admin")]
    public class KartelaModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IMapper _mapper;
        public string WarehouseItemName { get; set; }
        public int WarehouseItemId { get; set; }
        public KartelaModel(GrKouk.WebApi.Data.ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> OnGetAsync(int warehouseItemId)
        {
            var warehouseItem = await _context.WarehouseItems.FirstOrDefaultAsync(x => x.Id == warehouseItemId);
            if (warehouseItem is null)
            {
                return NotFound();
            }

            WarehouseItemId = warehouseItemId;
            WarehouseItemName = warehouseItem.Name;
            LoadFilters();
            return Page();


        }
        private void LoadFilters()
        {

            var datePeriods = DateFilter.GetDateFiltersSelectList();
            ViewData["DataFilterValues"] = new SelectList(datePeriods, "Value", "Text");

            var pageFilterSize = PageFilter.GetPageSizeFiltersSelectList();
            ViewData["PageFilterSize"] = new SelectList(pageFilterSize, "Value", "Text");

            var dbCompanies = _context.Companies.OrderBy(p => p.Code).AsNoTracking();
            List<SelectListItem> companiesList = new List<SelectListItem>();
            companiesList.Add(new SelectListItem() { Value = 0.ToString(), Text = "{All Companies}" });
            foreach (var company in dbCompanies)
            {
                companiesList.Add(new SelectListItem() { Value = company.Id.ToString(), Text = company.Code });
            }
            ViewData["CompanyFilter"] = new SelectList(companiesList, "Value", "Text");
        }
    }
}