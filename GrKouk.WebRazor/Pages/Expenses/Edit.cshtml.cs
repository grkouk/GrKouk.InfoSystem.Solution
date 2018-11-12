using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.Expenses
{
    public class EditModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public EditModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public FinDiaryTransaction FinDiaryTransaction { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            FinDiaryTransaction = await _context.FinDiaryTransactions
                .Include(f => f.Company)
                .Include(f => f.CostCentre)
                .Include(f => f.FinTransCategory)
                .Include(f => f.RevenueCentre)
                .Include(f => f.Transactor).FirstOrDefaultAsync(m => m.Id == id);

            if (FinDiaryTransaction == null)
            {
                return NotFound();
            }
           ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Code");
           ViewData["CostCentreId"] = new SelectList(_context.CostCentres, "Id", "Code");
           ViewData["FinTransCategoryId"] = new SelectList(_context.FinTransCategories, "Id", "Code");
           ViewData["RevenueCentreId"] = new SelectList(_context.RevenueCentres, "Id", "Code");
           ViewData["TransactorId"] = new SelectList(_context.Transactors, "Id", "Id");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(FinDiaryTransaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FinDiaryTransactionExists(FinDiaryTransaction.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool FinDiaryTransactionExists(int id)
        {
            return _context.FinDiaryTransactions.Any(e => e.Id == id);
        }
    }
}
