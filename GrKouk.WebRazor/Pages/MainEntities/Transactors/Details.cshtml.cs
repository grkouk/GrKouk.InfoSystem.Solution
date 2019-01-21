using System.Linq;
using System.Threading.Tasks;
using GrKouk.InfoSystem.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GrKouk.WebRazor.Pages.MainEntities.Transactors
{
    public class DetailsModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private int _id;

        public string Total
        {
            get
            {
                string _total;

                _total=_context.FinDiaryTransactions.Where(p => p.TransactorId == _id).Sum(p => p.AmountNet).ToString();
                return _total;
            }
            set { }
        } 
        public DetailsModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        public Transactor Transactor { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _id = (int)id;
            Transactor = await _context.Transactors
                .Include(t => t.TransactorType).FirstOrDefaultAsync(m => m.Id == id);

            if (Transactor == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
