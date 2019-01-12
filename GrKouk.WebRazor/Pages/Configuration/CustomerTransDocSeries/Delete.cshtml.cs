using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.Configuration.CustomerTransDocSeries
{
    public class DeleteModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public DeleteModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TransCustomerDocSeriesDef TransCustomerDocSeriesDef { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TransCustomerDocSeriesDef = await _context.TransCustomerDocSeriesDefs
                .Include(t => t.Company)
                .Include(t => t.TransCustomerDocTypeDef).FirstOrDefaultAsync(m => m.Id == id);

            if (TransCustomerDocSeriesDef == null)
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

            TransCustomerDocSeriesDef = await _context.TransCustomerDocSeriesDefs.FindAsync(id);

            if (TransCustomerDocSeriesDef != null)
            {
                _context.TransCustomerDocSeriesDefs.Remove(TransCustomerDocSeriesDef);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
