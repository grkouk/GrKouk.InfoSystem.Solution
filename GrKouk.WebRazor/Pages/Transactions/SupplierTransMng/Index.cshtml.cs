using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.Transactions.SupplierTransMng
{
    public class IndexModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public IndexModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        public IList<SupplierTransaction> SupplierTransaction { get;set; }

        public async Task OnGetAsync()
        {
            SupplierTransaction = await _context.SupplierTransactions
                .Include(s => s.Company)
                .Include(s => s.FiscalPeriod)
                .Include(s => s.FpaDef)
                .Include(s => s.Section)
                .Include(s => s.Supplier)
                .Include(s => s.TransSupplierDocSeries)
                .Include(s => s.TransSupplierDocType).ToListAsync();
        }
    }
}
