using System.Linq;
using System.Threading.Tasks;
using GrKouk.InfoSystem.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace GrKouk.WebRazor.Pages.Transactions.CustomerTransMng
{
    public class DeleteModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IToastNotification _toastNotification;
        private const string _sectionCode = "SYS-CUSTOMER-TRANS";
        public bool NotUpdatable;

        public DeleteModel(GrKouk.WebApi.Data.ApiDbContext context,IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        [BindProperty]
        public CustomerTransaction CustomerTransaction { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CustomerTransaction = await _context.CustomerTransactions
                .Include(s => s.Company)
                .Include(s => s.FiscalPeriod)
            
                .Include(s => s.Section)
                .Include(s => s.Customer)
                .Include(s => s.TransCustomerDocSeries)
                .Include(s => s.TransCustomerDocType).FirstOrDefaultAsync(m => m.Id == id);

            if (CustomerTransaction == null)
            {
                return NotFound();
            }
            var section = _context.Sections.SingleOrDefault(s => s.SystemName == _sectionCode);
            if (section is null)
            {
                _toastNotification.AddAlertToastMessage("Customer Transactions section not found in DB");
                return BadRequest();
            }
            //If section is not our section the canot update disable input controls
            NotUpdatable = CustomerTransaction.SectionId != section.Id;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CustomerTransaction = await _context.CustomerTransactions.FindAsync(id);

            if (CustomerTransaction != null)
            {
                _context.CustomerTransactions.Remove(CustomerTransaction);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
