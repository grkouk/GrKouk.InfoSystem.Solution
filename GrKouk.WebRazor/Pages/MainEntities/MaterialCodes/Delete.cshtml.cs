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
        public WarehouseItemCode WarehouseItemCode { get; set; }

        public async Task<IActionResult> OnGetAsync(WarehouseItemCodeTypeEnum codeType, int materialId, string code )
        {
            if (codeType == null)
            {
                return NotFound();
            }
            if (materialId == null)
            {
                return NotFound();
            }
            if (code == null)
            {
                return NotFound();
            }
            WarehouseItemCode = await _context.WarehouseItemsCodes
                .Include(m => m.WarehouseItem).FirstOrDefaultAsync(m => m.CodeType == codeType && m.WarehouseItemId==materialId && m.Code==code);

            if (WarehouseItemCode == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(WarehouseItemCodeTypeEnum codeType, int materialId, string code)
        {
            if (codeType == null)
            {
                return NotFound();
            }
            if (materialId == null)
            {
                return NotFound();
            }
            if (code == null)
            {
                return NotFound();
            }

            WarehouseItemCode = await _context.WarehouseItemsCodes.FindAsync( codeType, materialId , code);

            if (WarehouseItemCode != null)
            {
                _context.WarehouseItemsCodes.Remove(WarehouseItemCode);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
