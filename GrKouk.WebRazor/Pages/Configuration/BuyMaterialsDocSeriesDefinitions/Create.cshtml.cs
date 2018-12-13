using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.Configuration.BuyMaterialsDocSeriesDefinitions
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
        ViewData["BuyMaterialDocTypeDefId"] = new SelectList(_context.BuyMaterialDocTypeDefs, "Id", "Code");
        ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Code");
            return Page();
        }

        [BindProperty]
        public BuyMaterialDocSeriesDef BuyMaterialDocSeriesDef { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.BuyMaterialDocSeriesDefs.Add(BuyMaterialDocSeriesDef);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}