using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.Transactions.BuyMaterialsDoc
{
    public class DeleteModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public DeleteModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BuyMaterialsDocument BuyMaterialsDocument { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BuyMaterialsDocument = await _context.BuyMaterialsDocuments
                .Include(b => b.Company)
                .Include(b => b.FiscalPeriod)
                .Include(b => b.MaterialDocSeries)
                .Include(b => b.MaterialDocType)
                .Include(b => b.Section)
                .Include(b => b.Supplier).FirstOrDefaultAsync(m => m.Id == id);

            if (BuyMaterialsDocument == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            const string sectionCode = "SYS-BUY-MATERIALS-SCN";
            if (id == null)
            {
                return NotFound();
            }
            #region Section Management

            var section = await _context.Sections.SingleOrDefaultAsync(s => s.SystemName == sectionCode);
            if (section == null)
            {
               
               
                return NotFound(new
                {
                    error = "Could not locate section "
                });
            }

            #endregion
            BuyMaterialsDocument = await _context.BuyMaterialsDocuments.FindAsync(id);

            if (BuyMaterialsDocument != null)
            {
                _context.BuyMaterialsDocLines.RemoveRange(_context.BuyMaterialsDocLines.Where(p => p.BuyDocumentId == id));
                _context.SupplierTransactions.RemoveRange(_context.SupplierTransactions.Where(p => p.SectionId == section.Id && p.CreatorId == id));
                _context.WarehouseTransactions.RemoveRange(_context.WarehouseTransactions.Where(p => p.SectionId == section.Id && p.CreatorId == id));

                _context.BuyMaterialsDocuments.Remove(BuyMaterialsDocument);

                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
