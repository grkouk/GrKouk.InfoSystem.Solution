using System.Collections.Generic;
using System.Threading.Tasks;
using GrKouk.InfoSystem.Domain.FinConfig;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GrKouk.WebRazor.Pages.Configuration.BuyDocSeriesDefinitions
{
    public class IndexModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public IndexModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        public IList<BuyDocSeriesDef> BuyMaterialDocSeriesDef { get;set; }

        public async Task OnGetAsync()
        {
            BuyMaterialDocSeriesDef = await _context.BuyDocSeriesDefs
                .Include(b => b.BuyDocTypeDef)
                .Include(b => b.Company).ToListAsync();
        }
    }
}
