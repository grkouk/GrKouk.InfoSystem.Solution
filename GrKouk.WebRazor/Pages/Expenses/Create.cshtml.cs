using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GrKouk.InfoSystem.Definitions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace GrKouk.WebRazor.Pages.Expenses
{
    public class CreateModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;

        public CreateModel(GrKouk.WebApi.Data.ApiDbContext context, IMapper mapper, IToastNotification toastNotification)
        {
            _context = context;
            _mapper = mapper;
            _toastNotification = toastNotification;
        }

        public IActionResult OnGet()
        {
            LoadCompbos();
            return Page();
        }

        [BindProperty]
        public FinDiaryExpenceTransModifyDto FinDiaryTransaction { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                LoadCompbos();
                return Page();
            }

            var diaryTransactionToAttach = _mapper.Map<FinDiaryTransaction>(FinDiaryTransaction);

            diaryTransactionToAttach.Kind = (int)DiaryTransactionsKindEnum.Expence;
            diaryTransactionToAttach.RevenueCentreId = 1;

            _context.FinDiaryTransactions.Add(diaryTransactionToAttach);
            await _context.SaveChangesAsync();
            _toastNotification.AddSuccessToastMessage("Saved!");
            return RedirectToPage("./Index3");
        }

        private void LoadCompbos()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(c => c.Code).AsNoTracking(), "Id", "Code");
            ViewData["CostCentreId"] = new SelectList(_context.CostCentres.OrderBy(c => c.Name).AsNoTracking(), "Id", "Name");
            ViewData["FinTransCategoryId"] = new SelectList(_context.FinTransCategories.OrderBy(c => c.Name).AsNoTracking(), "Id", "Name");
           // ViewData["RevenueCentreId"] = new SelectList(_context.RevenueCentres.OrderBy(c => c.Name).AsNoTracking(), "Id", "Name");
            var transactorList = _context.Transactors.Where(s => s.TransactorType.Code == "SYS.DTRANSACTOR").OrderBy(s => s.Name).AsNoTracking();
            ViewData["TransactorId"] = new SelectList(transactorList, "Id", "Name");

        }
    }
}