using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.Transactions.BuyMaterialsDoc
{
    public class DetailsModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public DetailsModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

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
            return Page();
        }
    }
}
