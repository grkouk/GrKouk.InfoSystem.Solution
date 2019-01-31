using System.Linq;
using System.Threading.Tasks;
using GrKouk.InfoSystem.Domain.FinConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace GrKouk.WebRazor.Pages.Configuration.BuyDocSeriesDefinitions
{
    public class CreateModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IToastNotification _toastNotification;

        public CreateModel(GrKouk.WebApi.Data.ApiDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            this._toastNotification = toastNotification;
        }

        public IActionResult OnGet()
        {
            LoadCombos();
            return Page();
        }

        private void LoadCombos()
        {
            ViewData["BuyDocTypeDefId"] = new SelectList(_context.BuyDocTypeDefs.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(p => p.Code).AsNoTracking(), "Id", "Code");
        }

        [BindProperty]
        public BuyDocSeriesDef BuyDocSeriesDef { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.BuyDocSeriesDefs.Add(BuyDocSeriesDef);
            await _context.SaveChangesAsync();
            _toastNotification.AddInfoToastMessage("Saved");
            return RedirectToPage("./Index");
        }
    }
}