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

namespace GrKouk.WebRazor.Pages.Configuration.WarehouseTransDef
{
    public class EditModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public EditModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TransWarehouseDef TransWarehouseDef { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TransWarehouseDef = await _context.TransWarehouseDefs
                .Include(t => t.AmtBuyTrans)
                .Include(t => t.AmtExportsTrans)
                .Include(t => t.AmtImportsTrans)
                .Include(t => t.AmtInvoicedExportsTrans)
                .Include(t => t.AmtInvoicedImportsTrans)
                .Include(t => t.AmtSellTrans)
                .Include(t => t.Company)
                .Include(t => t.VolBuyTrans)
                .Include(t => t.VolExportsTrans)
                .Include(t => t.VolImportsTrans)
                .Include(t => t.VolInvoicedExportsTrans)
                .Include(t => t.VolInvoicedImportsTrans)
                .Include(t => t.VolSellTrans).FirstOrDefaultAsync(m => m.Id == id);

            if (TransWarehouseDef == null)
            {
                return NotFound();
            }
           ViewData["AmtBuyTransId"] = new SelectList(_context.FinancialMovements.AsNoTracking(), "Id", "Name");
           ViewData["AmtExportsTransId"] = new SelectList(_context.FinancialMovements.AsNoTracking(), "Id", "Name");
           ViewData["AmtImportsTransId"] = new SelectList(_context.FinancialMovements.AsNoTracking(), "Id", "Name");
           ViewData["AmtInvoicedExportsTransId"] = new SelectList(_context.FinancialMovements.AsNoTracking(), "Id", "Name");
           ViewData["AmtInvoicedImportsTransId"] = new SelectList(_context.FinancialMovements.AsNoTracking(), "Id", "Name");
           ViewData["AmtSellTransId"] = new SelectList(_context.FinancialMovements.AsNoTracking(), "Id", "Name");
           ViewData["CompanyId"] = new SelectList(_context.Companies.AsNoTracking(), "Id", "Code");
           ViewData["VolBuyTransId"] = new SelectList(_context.FinancialMovements.AsNoTracking(), "Id", "Name");
           ViewData["VolExportsTransId"] = new SelectList(_context.FinancialMovements.AsNoTracking(), "Id", "Name");
           ViewData["VolImportsTransId"] = new SelectList(_context.FinancialMovements.AsNoTracking(), "Id", "Name");
           ViewData["VolInvoicedExportsTransId"] = new SelectList(_context.FinancialMovements.AsNoTracking(), "Id", "Name");
           ViewData["VolInvoicedImportsTransId"] = new SelectList(_context.FinancialMovements.AsNoTracking(), "Id", "Name");
           ViewData["VolSellTransId"] = new SelectList(_context.FinancialMovements.AsNoTracking(), "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(TransWarehouseDef).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransWarehouseDefExists(TransWarehouseDef.Id))
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

        private bool TransWarehouseDefExists(int id)
        {
            return _context.TransWarehouseDefs.Any(e => e.Id == id);
        }
    }
}
