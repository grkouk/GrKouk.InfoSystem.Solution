using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.Configuration.BuyMaterialsDocSeriesDefinitions
{
    public class DeleteModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public DeleteModel(GrKouk.WebApi.Data.ApiDbContext context)
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BuyMaterialDocSeriesDef = await _context.BuyMaterialDocSeriesDefs.FindAsync(id);

            if (BuyMaterialDocSeriesDef != null)
            {
                _context.BuyMaterialDocSeriesDefs.Remove(BuyMaterialDocSeriesDef);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
