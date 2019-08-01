using System;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.InfoSystem.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace GrKouk.WebRazor.Pages.MainEntities.Transactors
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IToastNotification _toastNotification;

        public CreateModel(GrKouk.WebApi.Data.ApiDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        public IActionResult OnGet()
        {
            LoadCombos();
            return Page();
        }
        private void LoadCombos()
        {
            ViewData["TransactorTypeId"] = new SelectList(_context.TransactorTypes.OrderBy(p => p.Code).AsNoTracking(), "Id", "Code");
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(p => p.Code).AsNoTracking(), "Id", "Code");
        }
        [BindProperty]
        public Transactor Transactor { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Transactors.Add(Transactor);

            try
            {
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Transactor saved!");
            }
            catch (Exception e)
            {
                _toastNotification.AddErrorToastMessage(e.Message);
                Console.WriteLine(e);
            }

            return RedirectToPage("./Index");
        }
    }
}