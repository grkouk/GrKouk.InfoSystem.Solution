using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.InfoSystem.Definitions;
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
        public WarehouseItemCode WarehouseItemCode { get; set; }

        public async Task<IActionResult> OnGetAsync(WarehouseItemCodeTypeEnum id)
        {
           

            WarehouseItemCode = await _context.WarehouseItemsCodes
                .Include(m => m.WarehouseItem).FirstOrDefaultAsync(m => m.CodeType == id);

            if (WarehouseItemCode == null)
            {
                return NotFound();
            }
           ViewData["WarehouseItemId"] = new SelectList(_context.WarehouseItems, "Id", "Id");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(WarehouseItemCode).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaterialCodeExists(WarehouseItemCode.CodeType))
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

        private bool MaterialCodeExists(WarehouseItemCodeTypeEnum id)
        {
            return _context.WarehouseItemsCodes.Any(e => e.CodeType == id);
        }
    }
}
