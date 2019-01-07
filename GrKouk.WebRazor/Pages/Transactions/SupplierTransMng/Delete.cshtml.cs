using System.Threading.Tasks;
using  System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using NToastNotify;

namespace GrKouk.WebRazor.Pages.Transactions.SupplierTransMng
{
    public class DeleteModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IToastNotification _toastNotification;
        private const string SupplierTransSectionCode = "SYS-SUPPLIER-TRANS";
        public bool NotUpdatable;

        public DeleteModel(GrKouk.WebApi.Data.ApiDbContext context,IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
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
            
                .Include(s => s.Section)
                .Include(s => s.Supplier)
                .Include(s => s.TransSupplierDocSeries)
                .Include(s => s.TransSupplierDocType).FirstOrDefaultAsync(m => m.Id == id);

            if (SupplierTransaction == null)
            {
                return NotFound();
            }
            var section = _context.Sections.SingleOrDefault(s => s.SystemName == SupplierTransSectionCode);
            if (section is null)
            {
                _toastNotification.AddAlertToastMessage("Supplier Transactions section not found in DB");
                return BadRequest();
            }
            //If section is not our section the canot update disable input controls
            NotUpdatable = SupplierTransaction.SectionId != section.Id;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SupplierTransaction = await _context.SupplierTransactions.FindAsync(id);

            if (SupplierTransaction != null)
            {
                _context.SupplierTransactions.Remove(SupplierTransaction);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
