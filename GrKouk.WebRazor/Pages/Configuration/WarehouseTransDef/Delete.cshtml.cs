using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.Configuration.WarehouseTransDef
{
    public class DeleteModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public DeleteModel(GrKouk.WebApi.Data.ApiDbContext context)
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TransWarehouseDef = await _context.TransWarehouseDefs.FindAsync(id);

            if (TransWarehouseDef != null)
            {
                _context.TransWarehouseDefs.Remove(TransWarehouseDef);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
