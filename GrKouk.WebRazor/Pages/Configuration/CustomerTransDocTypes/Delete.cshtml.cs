﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.Configuration.CustomerTransDocTypes
{
    public class DeleteModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public DeleteModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TransCustomerDocTypeDef TransCustomerDocTypeDef { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TransCustomerDocTypeDef = await _context.TransCustomerDocTypeDefs
                .Include(t => t.Company)
                .Include(t => t.TransCustomerDef).FirstOrDefaultAsync(m => m.Id == id);

            if (TransCustomerDocTypeDef == null)
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

            TransCustomerDocTypeDef = await _context.TransCustomerDocTypeDefs.FindAsync(id);

            if (TransCustomerDocTypeDef != null)
            {
                _context.TransCustomerDocTypeDefs.Remove(TransCustomerDocTypeDef);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}