using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.FinConfig;

namespace GrKouk.WebRazor.Pages.Configuration.CustomerTransDocTypes
{
    public class IndexModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public IndexModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        public IList<TransCustomerDocTypeDef> TransCustomerDocTypeDef { get;set; }

        public async Task OnGetAsync()
        {
            TransCustomerDocTypeDef = await _context.TransCustomerDocTypeDefs
                .Include(t => t.Company)
                .Include(t => t.TransCustomerDef).ToListAsync();
        }
    }
}
