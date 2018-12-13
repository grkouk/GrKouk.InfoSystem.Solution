using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.Configuration
{
    public class DeleteModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public DeleteModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InfoSystem.Domain.FinConfig.BuyMaterialDocTypeDef BuyMaterialDocTypeDef { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BuyMaterialDocTypeDef = await _context.BuyMaterialDocTypeDefs
                .Include(b => b.Company)
                .Include(b => b.TransSupplierDef)
                .Include(b => b.TransWarehouseDef).FirstOrDefaultAsync(m => m.Id == id);

            if (BuyMaterialDocTypeDef == null)
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

            BuyMaterialDocTypeDef = await _context.BuyMaterialDocTypeDefs.FindAsync(id);

            if (BuyMaterialDocTypeDef != null)
            {
                _context.BuyMaterialDocTypeDefs.Remove(BuyMaterialDocTypeDef);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
