using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.Expenses
{
    public class DeleteModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public DeleteModel(GrKouk.WebApi.Data.ApiDbContext context)
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            FinDiaryTransaction = await _context.FinDiaryTransactions.FindAsync(id);

            if (FinDiaryTransaction != null)
            {
                _context.FinDiaryTransactions.Remove(FinDiaryTransaction);
              //  _context.FinDiaryTransactions.RemoveRange();
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
