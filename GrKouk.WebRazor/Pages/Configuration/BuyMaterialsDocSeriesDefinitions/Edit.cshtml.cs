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

namespace GrKouk.WebRazor.Pages.Configuration.BuyMaterialsDocSeriesDefinitions
{
    public class EditModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public EditModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BuyMaterialDocSeriesDef BuyMaterialDocSeriesDef { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BuyMaterialDocSeriesDef = await _context.BuyMaterialDocSeriesDefs
                .Include(b => b.BuyMaterialDocTypeDef)
                .Include(b => b.Company).FirstOrDefaultAsync(m => m.Id == id);

            if (BuyMaterialDocSeriesDef == null)
            {
                return NotFound();
            }
           ViewData["BuyMaterialDocTypeDefId"] = new SelectList(_context.BuyMaterialDocTypeDefs, "Id", "Code");
           ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Code");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(BuyMaterialDocSeriesDef).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BuyMaterialDocSeriesDefExists(BuyMaterialDocSeriesDef.Id))
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

        private bool BuyMaterialDocSeriesDefExists(int id)
        {
            return _context.BuyMaterialDocSeriesDefs.Any(e => e.Id == id);
        }
    }
}
