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
    public class DetailsModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public DetailsModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

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
    }
}
