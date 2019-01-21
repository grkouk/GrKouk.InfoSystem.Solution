using System.Collections.Generic;
using System.Threading.Tasks;
using GrKouk.InfoSystem.Domain.FinConfig;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GrKouk.WebRazor.Pages.Configuration.FinancialMovements
{
    public class IndexModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public IndexModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        public IList<FinancialMovement> FinancialMovement { get;set; }

        public async Task OnGetAsync()
        {
            FinancialMovement = await _context.FinancialMovements.ToListAsync();
        }
    }
}
