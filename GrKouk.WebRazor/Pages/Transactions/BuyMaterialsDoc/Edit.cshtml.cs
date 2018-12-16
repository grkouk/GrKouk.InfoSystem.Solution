using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.Transactions.BuyMaterialsDoc
{
    public class EditModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public EditModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BuyMaterialsDocument BuyMaterialsDocument { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BuyMaterialsDocument = await _context.BuyMaterialsDocuments
                .Include(b => b.Company)
                .Include(b => b.FiscalPeriod)
                .Include(b => b.MaterialDocSeries)
                .Include(b => b.MaterialDocType)
                .Include(b => b.Section)
                .Include(b => b.Supplier).FirstOrDefaultAsync(m => m.Id == id);

            if (BuyMaterialsDocument == null)
            {
                return NotFound();
            }
           ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Code");
           ViewData["FiscalPeriodId"] = new SelectList(_context.FiscalPeriods, "Id", "Id");
           ViewData["MaterialDocSeriesId"] = new SelectList(_context.BuyMaterialDocSeriesDefs, "Id", "Code");
           ViewData["MaterialDocTypeId"] = new SelectList(_context.BuyMaterialDocTypeDefs, "Id", "Code");
           ViewData["SectionId"] = new SelectList(_context.Sections, "Id", "Code");
           ViewData["SupplierId"] = new SelectList(_context.Transactors, "Id", "Id");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(BuyMaterialsDocument).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BuyMaterialsDocumentExists(BuyMaterialsDocument.Id))
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

        private bool BuyMaterialsDocumentExists(int id)
        {
            return _context.BuyMaterialsDocuments.Any(e => e.Id == id);
        }
    }
}
