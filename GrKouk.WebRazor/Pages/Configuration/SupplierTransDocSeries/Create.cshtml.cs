using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.Configuration.SupplierTransDocSeries
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
        ViewData["TransSupplierDocTypeDefId"] = new SelectList(_context.TransSupplierDocTypeDefs, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public TransSupplierDocSeriesDef TransSupplierDocSeriesDef { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.TransSupplierDocSeriesDefs.Add(TransSupplierDocSeriesDef);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}