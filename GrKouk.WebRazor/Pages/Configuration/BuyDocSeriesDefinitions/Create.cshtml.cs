﻿using System;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.InfoSystem.Definitions;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.WebRazor.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace GrKouk.WebRazor.Pages.Configuration.BuyDocSeriesDefinitions
{
    public class CreateModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IToastNotification _toastNotification;

        public CreateModel(GrKouk.WebApi.Data.ApiDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            this._toastNotification = toastNotification;
        }

        public IActionResult OnGet()
        {
            LoadCombos();
            return Page();
        }

        private void LoadCombos()
        {
            var seriesAutoPayOffList = Enum.GetValues(typeof(SeriesAutoPayoffEnum))
                .Cast<SeriesAutoPayoffEnum>()
                .Select(c => new SelectListItem()
                {
                    Value = c.ToString(),
                    Text = c.GetDescription()
                }).ToList();
            ViewData["AutoPayoffWay"]= new SelectList(seriesAutoPayOffList, "Value", "Text");
            ViewData["BuyDocTypeDefId"] = new SelectList(_context.BuyDocTypeDefs.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(p => p.Code).AsNoTracking(), "Id", "Code");
            ViewData["PayoffSeriesId"] = new SelectList(_context.TransTransactorDocSeriesDefs.OrderBy(s => s.Name).AsNoTracking(), "Id", "Name");
        }

        [BindProperty]
        public BuyDocSeriesDef BuyDocSeriesDef { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.BuyDocSeriesDefs.Add(BuyDocSeriesDef);
            await _context.SaveChangesAsync();
            _toastNotification.AddInfoToastMessage("Saved");
            return RedirectToPage("./Index");
        }
    }
}