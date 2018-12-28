using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos.WebDtos.SupplierTransactions;
using AutoMapper;
using GrKouk.InfoSystem.Domain.FinConfig;
using NToastNotify;
using Microsoft.EntityFrameworkCore;
using static GrKouk.InfoSystem.Domain.FinConfig.FinancialTransactionTypeEnum;

namespace GrKouk.WebRazor.Pages.Transactions.SupplierTransMng
{
    public class CreateModel : PageModel
    {
        private const string _supplierTransSectionCode = "SYS-SUPPLIER-TRANS";

        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IMapper mapper;
        private readonly IToastNotification toastNotification;

        public CreateModel(GrKouk.WebApi.Data.ApiDbContext context, IMapper mapper, IToastNotification toastNotification)
        {
            _context = context;
            this.mapper = mapper;
            this.toastNotification = toastNotification;
        }

        public IActionResult OnGet()
        {
            
            LoadCombos();
            return Page();
        }

        private void LoadCombos()
        {
            var supplierList = _context.Transactors.Where(s => s.TransactorType.Code == "SYS.SUPPLIER").OrderBy(s => s.Name).AsNoTracking();
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(c => c.Code).AsNoTracking(), "Id", "Code");
            ViewData["FiscalPeriodId"] = new SelectList(_context.FiscalPeriods.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
            ViewData["FpaDefId"] = new SelectList(_context.FpaKategories.OrderBy(c => c.Code).AsNoTracking(), "Id", "Code");
            ViewData["SupplierId"] = new SelectList(supplierList, "Id", "Name");
            ViewData["TransSupplierDocSeriesId"] = new SelectList(_context.TransSupplierDocSeriesDefs.OrderBy(s => s.Name).AsNoTracking(), "Id", "Name");
        }
       
        [BindProperty]
        public SupplierTransactionCreateDto SupplierTransactionDto { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                LoadCombos();
                return Page();
                //return RedirectToPage("./create");
            }

            var spTransaction = mapper.Map<SupplierTransaction>(SupplierTransactionDto);

            var docSeries = await
                _context.TransSupplierDocSeriesDefs.SingleOrDefaultAsync(m =>
                    m.Id == SupplierTransactionDto.TransSupplierDocSeriesId);

            if (docSeries is null)
            {
                ModelState.AddModelError(string.Empty, "Δεν βρέθηκε η σειρά παραστατικού");
                LoadCombos();
                return Page();
            }
            await _context.Entry(docSeries).Reference(t => t.TransSupplierDocTypeDef).LoadAsync();

            var docTypeDef = docSeries.TransSupplierDocTypeDef;
            await _context.Entry(docTypeDef)
                .Reference(t => t.TransSupplierDef)
                .LoadAsync();

            var transSupplierDef = docTypeDef.TransSupplierDef;
           

            var section =  await _context.Sections.SingleOrDefaultAsync(s => s.SystemName == _supplierTransSectionCode);
            if (section == null)
            {

                ModelState.AddModelError(string.Empty, "Δεν υπάρχει το Section");
                LoadCombos();
                return Page();
            }

            spTransaction.SectionId = section.Id;
            spTransaction.TransSupplierDocTypeId = docSeries.TransSupplierDocTypeDefId;
            spTransaction.FiscalPeriodId = 1;
            switch (transSupplierDef.FinancialTransType)
            {
                case InfoSystem.Domain.FinConfig.FinancialTransTypeEnum.FinancialTransTypeNoChange:
                    break;
                case InfoSystem.Domain.FinConfig.FinancialTransTypeEnum.FinancialTransTypeDebit:
                    spTransaction.FinancialAction = FinancialTransTypeEnum.FinancialTransTypeDebit;
                    spTransaction.TransactionType = FinancialTransactionTypeDebit;
                    break;
                case InfoSystem.Domain.FinConfig.FinancialTransTypeEnum.FinancialTransTypeCredit:
                    spTransaction.FinancialAction = FinancialTransTypeEnum.FinancialTransTypeCredit;
                    spTransaction.TransactionType = FinancialTransactionTypeCredit;
                    break;
                case InfoSystem.Domain.FinConfig.FinancialTransTypeEnum.FinancialTransTypeNegativeDebit:
                    spTransaction.FinancialAction = FinancialTransTypeEnum.FinancialTransTypeNegativeDebit;
                    spTransaction.TransactionType = FinancialTransactionTypeDebit;
                    spTransaction.AmountNet = spTransaction.AmountNet * -1;
                    spTransaction.AmountFpa = spTransaction.AmountFpa * -1;
                    spTransaction.AmountDiscount = spTransaction.AmountDiscount * -1;
                    break;
                case InfoSystem.Domain.FinConfig.FinancialTransTypeEnum.FinancialTransTypeNegativeCredit:
                    spTransaction.FinancialAction = FinancialTransTypeEnum.FinancialTransTypeNegativeCredit;
                    spTransaction.TransactionType = FinancialTransactionTypeCredit;
                    spTransaction.AmountNet = spTransaction.AmountNet * -1;
                    spTransaction.AmountFpa = spTransaction.AmountFpa * -1;
                    spTransaction.AmountDiscount = spTransaction.AmountDiscount * -1;
                    break;
                default:
                    break;
            }
           
           
            _context.SupplierTransactions.Add (spTransaction);
            await _context.SaveChangesAsync();
            toastNotification.AddSuccessToastMessage("Saved");
            return RedirectToPage("./Index");
        }
    }
}