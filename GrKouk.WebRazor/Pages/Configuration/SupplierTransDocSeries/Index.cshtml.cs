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
    public class IndexModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public IndexModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        public IList<TransSupplierDocSeriesDef> TransSupplierDocSeriesDef { get;set; }

        public async Task OnGetAsync()
        {
            TransSupplierDocSeriesDef = await _context.TransSupplierDocSeriesDefs
                .Include(t => t.Company)
                .Include(t => t.TransSupplierDocTypeDef).ToListAsync();
        }
    }
}
