using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.Configuration.SupplierTransDocSeries
{
    public class EditModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public EditModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TransSupplierDocSeriesDef TransSupplierDocSeriesDef { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TransSupplierDocSeriesDef = await _context.TransSupplierDocSeriesDefs
                .Include(t => t.Company)
                .Include(t => t.TransSupplierDocTypeDef).FirstOrDefaultAsync(m => m.Id == id);

            if (TransSupplierDocSeriesDef == null)
            {
                return NotFound();
            }
           ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Code");
           ViewData["TransSupplierDocTypeDefId"] = new SelectList(_context.TransSupplierDocTypeDefs, "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(TransSupplierDocSeriesDef).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransSupplierDocSeriesDefExists(TransSupplierDocSeriesDef.Id))
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

        private bool TransSupplierDocSeriesDefExists(int id)
        {
            return _context.TransSupplierDocSeriesDefs.Any(e => e.Id == id);
        }
    }
}
