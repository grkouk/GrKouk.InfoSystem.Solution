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
    public class IndexModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public IndexModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        public IList<InfoSystem.Domain.FinConfig.BuyMaterialDocTypeDef> BuyDocTypeDefs { get;set; }

        public async Task OnGetAsync()
        {
            BuyDocTypeDefs = await _context.BuyMaterialDocTypeDefs
                .Include(b => b.Company)
                .Include(b => b.TransSupplierDef)
                .Include(b => b.TransWarehouseDef)
                .ToListAsync();
        }
    }
}
