using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.CommonEntities.CrCashCatWarehouseItem
{
    public class IndexModel2 : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public IndexModel2(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        public IList<CrCatWarehouseItem> CrCatWarehouseItem { get;set; }

        public async Task OnGetAsync()
        {
            CrCatWarehouseItem = await _context.CrCatWarehouseItems
                .Include(c => c.CashRegCategory)
                .Include(c => c.ClientProfile)
                .Include(c => c.WarehouseItem).ToListAsync();
        }
    }
}
