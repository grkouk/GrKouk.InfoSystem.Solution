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

namespace GrKouk.WebRazor.Pages.Configuration.CustomerTransDocSeries
{
    public class EditModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public EditModel(GrKouk.WebApi.Data.ApiDbContext context)
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
          LoadCombos();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(TransCustomerDocSeriesDef).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransCustomerDocSeriesDefExists(TransCustomerDocSeriesDef.Id))
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

        private bool TransCustomerDocSeriesDefExists(int id)
        {
            return _context.TransCustomerDocSeriesDefs.Any(e => e.Id == id);
        }
        private void LoadCombos()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(p => p.Code).AsNoTracking(), "Id", "Code");
            ViewData["TransCustomerDocTypeDefId"] = new SelectList(_context.TransCustomerDocTypeDefs.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
        }
    }
}
