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
    public class DetailsModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public DetailsModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

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
    }
}
