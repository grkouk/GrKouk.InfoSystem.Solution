using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.Configuration.SupplierTransDocSeries
{
    public class DeleteModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public DeleteModel(GrKouk.WebApi.Data.ApiDbContext context)
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TransSupplierDocSeriesDef = await _context.TransSupplierDocSeriesDefs.FindAsync(id);

            if (TransSupplierDocSeriesDef != null)
            {
                _context.TransSupplierDocSeriesDefs.Remove(TransSupplierDocSeriesDef);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
