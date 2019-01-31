using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.InfoSystem.Domain.FinConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.MainEntities.MaterialCodes
{
    public class EditModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public EditModel(GrKouk.WebApi.Data.ApiDbContext context)
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
           ViewData["MaterialId"] = new SelectList(_context.Materials, "Id", "Id");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(MaterialCode).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaterialCodeExists(MaterialCode.CodeType))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MaterialCodeExists(MaterialCodeTypeEnum id)
        {
            return _context.MaterialCodes.Any(e => e.CodeType == id);
        }
    }
}
