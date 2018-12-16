using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.Transactions.BuyMaterialsDoc
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
        ViewData["FiscalPeriodId"] = new SelectList(_context.FiscalPeriods, "Id", "Id");
        ViewData["MaterialDocSeriesId"] = new SelectList(_context.BuyMaterialDocSeriesDefs, "Id", "Code");
        ViewData["MaterialDocTypeId"] = new SelectList(_context.BuyMaterialDocTypeDefs, "Id", "Code");
        ViewData["SectionId"] = new SelectList(_context.Sections, "Id", "Code");
        ViewData["SupplierId"] = new SelectList(_context.Transactors, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public BuyMaterialsDocument BuyMaterialsDocument { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.BuyMaterialsDocuments.Add(BuyMaterialsDocument);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}