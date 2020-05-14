using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.WebApi.Data;
using GrKouk.InfoSystem.Definitions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;


namespace GrKouk.WebRazor
{
    public class AppSettingsModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IToastNotification _toastNotification;

        public AppSettingsModel(ApiDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }
        [BindProperty]
        public IList<AppSetting> ItemVm { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            LoadCombos();
            ItemVm= new List<AppSetting>();
            var allCompanyCodeSetting = await _context.AppSettings.FirstOrDefaultAsync(p => p.Code == Constants.AllCompaniesCodeKey);
            if (allCompanyCodeSetting != null)
            {
                var acs = new AppSetting
                {
                    Code = allCompanyCodeSetting.Value,
                    
                };
                ItemVm.Add(acs);
            }
            else
            {
                ItemVm.Add(new AppSetting
                {
                    Code = "ALLCOMP"
                    
                });
            }

            return Page();
        }

        private void LoadCombos()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(p => p.Name).AsNoTracking(), "Code", "Name");
        }

        public async Task<IActionResult> OnPostAsync()
        {
           
            var allCompanyCodeSetting = await _context.AppSettings.FirstOrDefaultAsync(p => p.Code == Constants.AllCompaniesCodeKey);
            if (allCompanyCodeSetting != null)
            {
                allCompanyCodeSetting.Value=ItemVm[0].Code;
                _context.Attach(allCompanyCodeSetting).State = EntityState.Modified;
            }
            else
            {
                var newAllCompSetting = new AppSetting
                {
                    Code = Constants.AllCompaniesCodeKey,
                    Value = ItemVm[0].Code
                };
                _context.AppSettings.Add(newAllCompSetting);
            }

            try
            {
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Settings saved");
                return RedirectToPage("./AppSettings");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _toastNotification.AddErrorToastMessage(e.Message);
                return Page();
            }

           
        }
    }
}