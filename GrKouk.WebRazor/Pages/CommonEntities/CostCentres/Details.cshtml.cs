﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.WebApi.Data;
using Microsoft.AspNetCore.Authorization;

namespace GrKouk.WebRazor.Pages.CommonEntities.CostCentres
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public DetailsModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

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
    }
}
