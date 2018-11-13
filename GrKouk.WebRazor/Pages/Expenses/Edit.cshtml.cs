using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.Expenses
{
    public class EditModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IMapper _mapper;


        public EditModel(GrKouk.WebApi.Data.ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [BindProperty]
        public FinDiaryExpenceTransModifyDto FinDiaryTransactionVM { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var diaryTransactionToModify = await _context.FinDiaryTransactions
                .Include(f => f.Company)
                .Include(f => f.CostCentre)
                .Include(f => f.FinTransCategory)
                .Include(f => f.RevenueCentre)
                .Include(f => f.Transactor).FirstOrDefaultAsync(m => m.Id == id);

            if (diaryTransactionToModify == null)
            {
                return NotFound();
            }

            FinDiaryTransactionVM = _mapper.Map<FinDiaryExpenceTransModifyDto>(diaryTransactionToModify);

            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
            ViewData["CostCentreId"] = new SelectList(_context.CostCentres.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
            ViewData["FinTransCategoryId"] = new SelectList(_context.FinTransCategories.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
           // ViewData["RevenueCentreId"] = new SelectList(_context.RevenueCentres.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
            ViewData["TransactorId"] = new SelectList(_context.Transactors.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var diaryTransactionToAttach = _mapper.Map<FinDiaryTransaction>(FinDiaryTransactionVM);

            diaryTransactionToAttach.Kind = (int) DiaryTransactionsKinds.Expence;
            diaryTransactionToAttach.RevenueCentreId = 1;

            _context.Attach(diaryTransactionToAttach).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FinDiaryTransactionExists(diaryTransactionToAttach.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return RedirectToPage("./Index");
        }

        private bool FinDiaryTransactionExists(int id)
        {
            return _context.FinDiaryTransactions.Any(e => e.Id == id);
        }
    }
}
