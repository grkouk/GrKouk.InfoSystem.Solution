using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.Expenses
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
        ViewData["CostCentreId"] = new SelectList(_context.CostCentres, "Id", "Code");
        ViewData["FinTransCategoryId"] = new SelectList(_context.FinTransCategories, "Id", "Code");
        ViewData["RevenueCentreId"] = new SelectList(_context.RevenueCentres, "Id", "Code");
        ViewData["TransactorId"] = new SelectList(_context.Transactors, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public FinDiaryTransaction FinDiaryTransaction { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.FinDiaryTransactions.Add(FinDiaryTransaction);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}