using System.Linq;
using System.Threading.Tasks;
using GrKouk.InfoSystem.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GrKouk.WebRazor.Pages.Transactions.SellMaterialDoc
{
    public class DeleteModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public DeleteModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SellDocument SaleDocument { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SaleDocument = await _context.SellDocuments
                .Include(b => b.Company)
                .Include(b => b.FiscalPeriod)
                .Include(b => b.SellDocSeries)
                .Include(b => b.SellDocType)
                .Include(b => b.Section)
                .Include(b => b.Transactor).FirstOrDefaultAsync(m => m.Id == id);

            if (SaleDocument == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            const string sectionCode = "SYS-SELL-COMBINED-SCN";
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
            SaleDocument = await _context.SellDocuments.FindAsync(id);

            if (SaleDocument != null)
            {
                _context.SellDocLines.RemoveRange(_context.SellDocLines.Where(p => p.SellDocumentId == id));
                _context.TransactorTransactions.RemoveRange(_context.TransactorTransactions.Where(p => p.SectionId == section.Id && p.CreatorId == id));
                _context.WarehouseTransactions.RemoveRange(_context.WarehouseTransactions.Where(p => p.SectionId == section.Id && p.CreatorId == id));

                _context.SellDocuments.Remove(SaleDocument);

                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
