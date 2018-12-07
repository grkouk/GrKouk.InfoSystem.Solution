using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GrKouk.WebRazor.Pages.Configuration
{
    public class CreateModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public CreateModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;

        }

        public IActionResult OnGet()
        {
        ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Code");
        ViewData["TransSupplierDefId"] = new SelectList(_context.TransSupplierDefs, "Id", "Name");
        ViewData["TransWarehouseDefId"] = new SelectList(_context.TransWarehouseDefs, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public InfoSystem.Domain.FinConfig.BuyDocTypeDef BuyDocTypeDef { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.BuyDocTypeDefs.Add(BuyDocTypeDef);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}