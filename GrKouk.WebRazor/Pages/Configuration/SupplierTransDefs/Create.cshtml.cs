using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.Configuration.SupplierTransDefs
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
        ViewData["CreditTransId"] = new SelectList(_context.FinancialMovements, "Id", "Code");
        ViewData["DebitTransId"] = new SelectList(_context.FinancialMovements, "Id", "Code");
        ViewData["TurnOverTransId"] = new SelectList(_context.FinancialMovements, "Id", "Code");
            return Page();
        }

        [BindProperty]
        public TransSupplierDef TransSupplierDef { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.TransSupplierDefs.Add(TransSupplierDef);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}