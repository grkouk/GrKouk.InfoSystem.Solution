using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.InfoSystem.Definitions;
using GrKouk.InfoSystem.Domain.FinConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.WebApi.Data;
using Microsoft.AspNetCore.Authorization;

namespace GrKouk.WebRazor.Pages.MainEntities.MaterialCodes
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public DetailsModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        public WarehouseItemCode WarehouseItemCode { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            //WarehouseItemCode = await _context.WrItemCodes
            //    .Include(m => m.WarehouseItem).FirstOrDefaultAsync(m => m.Id == id);

            //if (WarehouseItemCode == null)
            //{
            //    return NotFound();
            //}
            return Page();
        }
    }
}
