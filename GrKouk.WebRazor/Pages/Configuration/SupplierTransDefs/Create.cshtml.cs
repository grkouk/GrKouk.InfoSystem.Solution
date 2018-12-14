using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.WebApi.Data;
using Microsoft.EntityFrameworkCore;

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
            NewMethod();
            return Page();
        }

        private void NewMethod()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(p => p.Code).AsNoTracking(), "Id", "Code");
            ViewData["CreditTransId"] = new SelectList(_context.FinancialMovements.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
            ViewData["DebitTransId"] = new SelectList(_context.FinancialMovements.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
            ViewData["TurnOverTransId"] = new SelectList(_context.FinancialMovements.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
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