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

        public async Task<IActionResult> OnGetAsync(MaterialCodeTypeEnum id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MaterialCode = await _context.MaterialCodes
                .Include(m => m.Material).FirstOrDefaultAsync(m => m.CodeType == id);

            if (MaterialCode == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(MaterialCodeTypeEnum id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MaterialCode = await _context.MaterialCodes.FindAsync(id);

            if (MaterialCode != null)
            {
                _context.MaterialCodes.Remove(MaterialCode);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
