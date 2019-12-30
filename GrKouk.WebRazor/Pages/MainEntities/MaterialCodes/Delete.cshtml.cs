using System.Threading.Tasks;
using GrKouk.InfoSystem.Definitions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using Microsoft.AspNetCore.Authorization;

namespace GrKouk.WebRazor.Pages.MainEntities.MaterialCodes
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public DeleteModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public WrItemCode WarehouseItemCode { get; set; }

        public async Task<IActionResult> OnGetAsync(int id )
        {
            if (id == 0)
            {
                return NotFound();
            }
            WarehouseItemCode = await _context.WrItemCodes
                .Include(m => m.WarehouseItem).FirstOrDefaultAsync(m => m.Id==id);

            if (WarehouseItemCode == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            WarehouseItemCode = await _context.WrItemCodes.FindAsync( id);

            if (WarehouseItemCode != null)
            {
                _context.WrItemCodes.Remove(WarehouseItemCode);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
