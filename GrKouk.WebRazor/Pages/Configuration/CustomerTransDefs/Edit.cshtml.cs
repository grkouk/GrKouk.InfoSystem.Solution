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

namespace GrKouk.WebRazor.Pages.Configuration.CustomerTransDefs
{
    public class EditModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public EditModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TransCustomerDef TransCustomerDef { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TransCustomerDef = await _context.TransCustomerDefs
                .Include(t => t.Company)
                .Include(t => t.TurnOverTrans).FirstOrDefaultAsync(m => m.Id == id);

            if (TransCustomerDef == null)
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

            _context.Attach(TransCustomerDef).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransCustomerDefExists(TransCustomerDef.Id))
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

        private bool TransCustomerDefExists(int id)
        {
            return _context.TransCustomerDefs.Any(e => e.Id == id);
        }
        private void LoadCombos()
        {
            List<SelectListItem> financialTransTypes = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Value = FinActionsEnum.FinActionsEnumNoChange.ToString(),
                    Text = "No Change"
                },
               new SelectListItem()
                {
                    Value = FinActionsEnum.FinActionsEnumDebit.ToString(),
                    Text = "Debit"
                },
               new SelectListItem()
               {
                   Value = FinActionsEnum.FinActionsEnumCredit.ToString(),
                   Text = "Credit"
               },

                new SelectListItem()
                {
                    Value = FinActionsEnum.FinActionsEnumNegativeDebit.ToString(),
                    Text = "Neg.Debit"
                },
                new SelectListItem()
                {
                    Value = FinActionsEnum.FinActionsEnumNegativeCredit.ToString(),
                    Text = "Neg.Credit"
                }
            };
            ViewData["FinancialTransTypes"] = new SelectList(financialTransTypes, "Value", "Text");
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(p => p.Code).AsNoTracking(), "Id", "Code");
            ViewData["TurnOverTransId"] = new SelectList(_context.FinancialMovements.AsNoTracking(), "Id", "Name");

            var dbSeriesList = _context.TransCustomerDocSeriesDefs.OrderBy(p => p.Name).AsNoTracking();
            List<SelectListItem> seriesList = new List<SelectListItem>();
            seriesList.Add(new SelectListItem() { Value = 0.ToString(), Text = "{No Default series}" });
            foreach (var dbSeriesItem in dbSeriesList)
            {
                seriesList.Add(new SelectListItem() { Value = dbSeriesItem.Id.ToString(), Text = dbSeriesItem.Name });
            }
            ViewData["DefaultDocSeriesId"] = new SelectList(seriesList, "Value", "Text");
        }
    }
}