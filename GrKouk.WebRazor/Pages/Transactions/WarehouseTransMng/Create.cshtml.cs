using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.Transactions.WarehouseTransMng
{
    public class CreateModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public CreateModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Code");
        ViewData["FiscalPeriodId"] = new SelectList(_context.FiscalPeriods, "Id", "Name");
        ViewData["MaterialId"] = new SelectList(_context.Materials, "Id", "Name");
        ViewData["SectionId"] = new SelectList(_context.Sections, "Id", "Code");
        ViewData["TransWarehouseDocSeriesId"] = new SelectList(_context.TransWarehouseDocSeriesDefs, "Id", "Name");
        ViewData["TransWarehouseDocTypeId"] = new SelectList(_context.TransWarehouseDocTypeDefs, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public WarehouseTransaction WarehouseTransaction { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.WarehouseTransactions.Add(WarehouseTransaction);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}