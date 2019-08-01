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
    public class EditModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IToastNotification _toastNotification;

        public EditModel(GrKouk.WebApi.Data.ApiDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        [BindProperty]
        public Transactor Transactor { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Transactor = await _context.Transactors
                .Include(t => t.TransactorType).FirstOrDefaultAsync(m => m.Id == id);

            if (Transactor == null)
            {
                return NotFound();
            }
            LoadCombos();

            return Page();
        }

        private void LoadCombos()
        {
            ViewData["TransactorTypeId"] = new SelectList(_context.TransactorTypes.OrderBy(p => p.Code).AsNoTracking(), "Id", "Code");
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(p => p.Code).AsNoTracking(), "Id", "Code");
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Transactor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Modifications saved!");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactorExists(Transactor.Id))
                {
                    return NotFound();
                }
                else
                {
                    _toastNotification.AddErrorToastMessage("Not saved concurrency exception.");
                }
            }
            catch (Exception e)
            {
                _toastNotification.AddErrorToastMessage(e.Message);
            }

            return RedirectToPage("./Index");
        }

        private bool TransactorExists(int id)
        {
            return _context.Transactors.Any(e => e.Id == id);
        }
    }
}
