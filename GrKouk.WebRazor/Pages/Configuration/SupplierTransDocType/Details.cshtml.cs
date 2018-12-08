using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.Configuration.SuuplierTransDocType
{
    public class DetailsModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public DetailsModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        public TransSupplierDocTypeDef TransSupplierDocTypeDef { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TransSupplierDocTypeDef = await _context.TransSupplierDocTypeDefs
                .Include(t => t.Company)
                .Include(t => t.TransSupplierDef).FirstOrDefaultAsync(m => m.Id == id);

            if (TransSupplierDocTypeDef == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
