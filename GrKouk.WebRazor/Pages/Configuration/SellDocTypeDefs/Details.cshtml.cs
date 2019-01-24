using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.Configuration.SellDocTypeDefs
{
    public class DetailsModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public DetailsModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        public SellDocTypeDef SellDocTypeDef { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SellDocTypeDef = await _context.SellDocTypeDefs
                .Include(s => s.Company)
                .Include(s => s.TransTransactorDef)
                .Include(s => s.TransWarehouseDef).FirstOrDefaultAsync(m => m.Id == id);

            if (SellDocTypeDef == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
