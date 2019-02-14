﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.Configuration.TransactorTransDocSeries
{
    public class EditModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public EditModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TransTransactorDocSeriesDef TransTransactorDocSeriesDef { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TransTransactorDocSeriesDef = await _context.TransTransactorDocSeriesDefs
                .Include(t => t.Company)
                .Include(t => t.TransTransactorDocTypeDef).FirstOrDefaultAsync(m => m.Id == id);

            if (TransTransactorDocSeriesDef == null)
            {
                return NotFound();
            }
          LoadCombos();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(TransTransactorDocSeriesDef).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransTransactorDocSeriesDefExists(TransTransactorDocSeriesDef.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TransTransactorDocSeriesDefExists(int id)
        {
            return _context.TransTransactorDocSeriesDefs.Any(e => e.Id == id);
        }
        private void LoadCombos()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(p => p.Code).AsNoTracking(), "Id", "Code");
            ViewData["TransTransactorDocTypeDefId"] = new SelectList(_context.TransTransactorDocTypeDefs.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
        }
    }
}
