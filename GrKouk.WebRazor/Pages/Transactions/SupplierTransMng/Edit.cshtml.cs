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

namespace GrKouk.WebRazor.Pages.Transactions.SupplierTransMng
{
    public class EditModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public EditModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SupplierTransaction SupplierTransaction { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SupplierTransaction = await _context.SupplierTransactions
                .Include(s => s.Company)
                .Include(s => s.FiscalPeriod)
                .Include(s => s.FpaDef)
                .Include(s => s.Section)
                .Include(s => s.Supplier)
                .Include(s => s.TransSupplierDocSeries)
                .Include(s => s.TransSupplierDocType).FirstOrDefaultAsync(m => m.Id == id);

            if (SupplierTransaction == null)
            {
                return NotFound();
            }
           ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Code");
           ViewData["FiscalPeriodId"] = new SelectList(_context.FiscalPeriods, "Id", "Name");
           ViewData["FpaDefId"] = new SelectList(_context.FpaKategories, "Id", "Code");
           ViewData["SectionId"] = new SelectList(_context.Sections, "Id", "Code");
           ViewData["SupplierId"] = new SelectList(_context.Transactors, "Id", "Name");
           ViewData["TransSupplierDocSeriesId"] = new SelectList(_context.TransSupplierDocSeriesDefs, "Id", "Name");
           ViewData["TransSupplierDocTypeId"] = new SelectList(_context.TransSupplierDocTypeDefs, "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(SupplierTransaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierTransactionExists(SupplierTransaction.Id))
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

        private bool SupplierTransactionExists(int id)
        {
            return _context.SupplierTransactions.Any(e => e.Id == id);
        }
    }
}
