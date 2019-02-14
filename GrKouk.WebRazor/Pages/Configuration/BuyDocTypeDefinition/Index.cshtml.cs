using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GrKouk.WebRazor.Pages.Configuration.BuyDocTypeDefinition
{
    public class IndexModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public IndexModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        public IList<InfoSystem.Domain.FinConfig.BuyDocTypeDef> BuyDocTypeDefs { get;set; }

        public async Task OnGetAsync()
        {
            BuyDocTypeDefs = await _context.BuyDocTypeDefs
                .Include(b => b.Company)
                .Include(b => b.TransTransactorDef)
                .Include(b => b.TransWarehouseDef)
                .ToListAsync();
        }
    }
}
