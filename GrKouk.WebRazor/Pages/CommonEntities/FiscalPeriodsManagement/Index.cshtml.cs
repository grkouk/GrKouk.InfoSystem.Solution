using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.CommonEntities.FiscalPeriodsManagement
{
    public class IndexModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public IndexModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        public IList<FiscalPeriod> FiscalPeriod { get;set; }

        public async Task OnGetAsync()
        {
            FiscalPeriod = await _context.FiscalPeriods.ToListAsync();
        }
    }
}
