using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GrKouk.InfoSystem.Domain.Shared;
using Microsoft.AspNetCore.Authorization;

namespace GrKouk.WebRazor.Pages.CommonEntities.MaterialCategories
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public CreateModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public MaterialCategory MaterialCategory { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.MaterialCategories.Add(MaterialCategory);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}