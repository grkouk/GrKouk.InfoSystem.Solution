﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.CommonEntities.CostCentres
{
    public class DeleteModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public DeleteModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CostCentre CostCentre { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CostCentre = await _context.CostCentres.FirstOrDefaultAsync(m => m.Id == id);

            if (CostCentre == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CostCentre = await _context.CostCentres.FindAsync(id);

            if (CostCentre != null)
            {
                _context.CostCentres.Remove(CostCentre);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
