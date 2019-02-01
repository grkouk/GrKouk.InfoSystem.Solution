using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.InfoSystem.Domain.FinConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.MainEntities.MaterialCodes
{
    public class DeleteModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public DeleteModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MaterialCode MaterialCode { get; set; }

        public async Task<IActionResult> OnGetAsync(MaterialCodeTypeEnum codeType, int materialId, string code )
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
            MaterialCode = await _context.MaterialCodes
                .Include(m => m.Material).FirstOrDefaultAsync(m => m.CodeType == codeType && m.MaterialId==materialId && m.Code==code);

            if (MaterialCode == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(MaterialCodeTypeEnum codeType, int materialId, string code)
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

            MaterialCode = await _context.MaterialCodes.FindAsync( codeType, materialId , code);

            if (MaterialCode != null)
            {
                _context.MaterialCodes.Remove(MaterialCode);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
