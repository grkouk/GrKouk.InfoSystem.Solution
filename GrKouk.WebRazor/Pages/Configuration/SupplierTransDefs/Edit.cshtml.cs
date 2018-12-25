using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.WebApi.Data;
using NToastNotify;

namespace GrKouk.WebRazor.Pages.Configuration.SupplierTransDefs
{
    public class EditModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IToastNotification toastNotification;

        public EditModel(GrKouk.WebApi.Data.ApiDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            this.toastNotification = toastNotification;
        }

        [BindProperty]
        public TransSupplierDef TransSupplierDef { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TransSupplierDef = await _context.TransSupplierDefs
                .Include(t => t.Company)
                .Include(t => t.CreditTrans)
                .Include(t => t.DebitTrans)
                .Include(t => t.TurnOverTrans).FirstOrDefaultAsync(m => m.Id == id);

            if (TransSupplierDef == null)
            {
                return NotFound();
            }
            LoadCombos();
            return Page();
        }

        private void LoadCombos()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(p => p.Code).AsNoTracking(), "Id", "Code");
            ViewData["CreditTransId"] = new SelectList(_context.FinancialMovements.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
            ViewData["DebitTransId"] = new SelectList(_context.FinancialMovements.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
            ViewData["TurnOverTransId"] = new SelectList(_context.FinancialMovements.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
            ViewData["TransSupplierDefaultDocSeriesId"] = new SelectList(_context.TransSupplierDocSeriesDefs.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(TransSupplierDef).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                toastNotification.AddSuccessToastMessage("Saved");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransSupplierDefExists(TransSupplierDef.Id))
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

        private bool TransSupplierDefExists(int id)
        {
            return _context.TransSupplierDefs.Any(e => e.Id == id);
        }
    }
}
