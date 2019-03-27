using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.MediaEntities;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.MediaMng
{
    public class DetailsModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public DetailsModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        public MediaEntry MediaEntry { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MediaEntry = await _context.MediaEntries.FirstOrDefaultAsync(m => m.Id == id);

            if (MediaEntry == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
