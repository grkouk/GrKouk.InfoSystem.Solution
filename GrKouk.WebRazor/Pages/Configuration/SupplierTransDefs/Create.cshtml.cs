using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.WebApi.Data;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace GrKouk.WebRazor.Pages.Configuration.SupplierTransDefs
{
    public class CreateModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IToastNotification toastNotification;

        public CreateModel(GrKouk.WebApi.Data.ApiDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            this.toastNotification = toastNotification;
        }

        public IActionResult OnGet()
        {
            LoadCombos();
            return Page();
        }

        private void LoadCombos()
        {
            List<SelectListItem> financialTransTypes = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Value = FinancialTransTypeEnum.FinancialTransTypeNoChange.ToString(),
                    Text = "No Change"
                },
               new SelectListItem()
                {
                    Value = FinancialTransTypeEnum.FinancialTransTypeDebit.ToString(),
                    Text = "Debit"
                },
               new SelectListItem()
               {
                   Value = FinancialTransTypeEnum.FinancialTransTypeCredit.ToString(),
                   Text = "Credit"
               },
               
                new SelectListItem()
                {
                    Value = FinancialTransTypeEnum.FinancialTransTypeNegativeDebit.ToString(),
                    Text = "Neg.Debit"
                },
                new SelectListItem()
                {
                    Value = FinancialTransTypeEnum.FinancialTransTypeNegativeCredit.ToString(),
                    Text = "Neg.Credit"
                }
            };
            ViewData["FinancialTransTypes"] = new SelectList(financialTransTypes, "Value", "Text");
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(p => p.Code).AsNoTracking(), "Id", "Code");
            ViewData["CreditTransId"] = new SelectList(_context.FinancialMovements.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
            ViewData["DebitTransId"] = new SelectList(_context.FinancialMovements.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
            ViewData["TurnOverTransId"] = new SelectList(_context.FinancialMovements.AsNoTracking(), "Id", "Name");

            var dbSeriesList = _context.TransSupplierDocSeriesDefs.OrderBy(p => p.Name).AsNoTracking();
            List<SelectListItem> seriesList = new List<SelectListItem>();
            seriesList.Add(new SelectListItem() { Value = 0.ToString(), Text = "{No Default series}" });
            foreach (var dbSeriesItem in dbSeriesList)
            {
                seriesList.Add(new SelectListItem() { Value = dbSeriesItem.Id.ToString(), Text = dbSeriesItem.Name });
            }
            ViewData["TransSupplierDefaultDocSeriesId"] = new SelectList(seriesList, "Value", "Text");
        }

        [BindProperty]
        public TransSupplierDef TransSupplierDef { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.TransSupplierDefs.Add(TransSupplierDef);
            await _context.SaveChangesAsync();
            toastNotification.AddSuccessToastMessage("Saved");
            return RedirectToPage("./Index");
        }
    }
}