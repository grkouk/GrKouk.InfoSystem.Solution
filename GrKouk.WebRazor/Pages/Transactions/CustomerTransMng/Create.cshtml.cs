using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos.WebDtos.CustomerTransactions;
using GrKouk.InfoSystem.Dtos.WebDtos.SupplierTransactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace GrKouk.WebRazor.Pages.Transactions.CustomerTransMng
{
    public class CreateModel : PageModel
    {
        private const string _SectionCode = "SYS-CUSTOMER-TRANS";

        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IMapper mapper;
        private readonly IToastNotification toastNotification;
       
        private bool _initialLoad =true;
        [BindProperty]
        public bool InitialLoad
        {
            get => _initialLoad;
            set => _initialLoad = value;
        }
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
            var customerList = _context.Transactors.Where(s => s.TransactorType.Code == "SYS.CUSTOMER").OrderBy(s => s.Name).AsNoTracking();
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(c => c.Code).AsNoTracking(), "Id", "Code");
            ViewData["FiscalPeriodId"] = new SelectList(_context.FiscalPeriods.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
            ViewData["FpaDefId"] = new SelectList(_context.FpaKategories.OrderBy(c => c.Code).AsNoTracking(), "Id", "Code");
            ViewData["CustomerId"] = new SelectList(customerList, "Id", "Name");
            ViewData["TransCustomerDocSeriesId"] = new SelectList(_context.TransCustomerDocSeriesDefs.OrderBy(s => s.Name).AsNoTracking(), "Id", "Name");
        }
       
        [BindProperty]
        public CustomerTransactionCreateDto ItemVm { get; set; }

       

        public async Task<IActionResult> OnPostAsync()
        {
            Debug.Print("Inside Post IsInitial Load value" + _initialLoad.ToString());
            if (!ModelState.IsValid)
            {
                LoadCombos();
                return Page();
            }
            #region Fiscal Period
            if (ItemVm.FiscalPeriodId <= 0)
            {
                ModelState.AddModelError(string.Empty, "No Fiscal Period covers Transaction Date");
                LoadCombos();
                return Page();
            }
            #endregion
            var spTransaction = mapper.Map<CustomerTransaction>(ItemVm);
           
            var docSeries = await
                _context.TransCustomerDocSeriesDefs.SingleOrDefaultAsync(m =>
                    m.Id == ItemVm.TransCustomerDocSeriesId);

            if (docSeries is null)
            {
                ModelState.AddModelError(string.Empty, "Δεν βρέθηκε η σειρά παραστατικού");
                LoadCombos();
                return Page();
            }
            await _context.Entry(docSeries).Reference(t => t.TransCustomerDocTypeDef).LoadAsync();

            var docTypeDef = docSeries.TransCustomerDocTypeDef;
            await _context.Entry(docTypeDef)
                .Reference(t => t.TransCustomerDef)
                .LoadAsync();

            var transCustomerDef = docTypeDef.TransCustomerDef;
           

            var section =  await _context.Sections.SingleOrDefaultAsync(s => s.SystemName == _SectionCode);
            if (section == null)
            {

                ModelState.AddModelError(string.Empty, "Δεν υπάρχει το Section");
                LoadCombos();
                return Page();
            }

            spTransaction.SectionId = section.Id;
            spTransaction.TransCustomerDocTypeId = docSeries.TransCustomerDocTypeDefId;
            //spTransaction.FiscalPeriodId = fiscalPeriod.Id;
            spTransaction.FinancialAction = transCustomerDef.FinancialTransAction;
            switch (transCustomerDef.FinancialTransAction)
            {
                case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumNoChange:
                    spTransaction.TransactionType = FinancialTransactionTypeEnum.FinancialTransactionTypeIgnore;
                    break;
                case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumDebit:
                    spTransaction.TransactionType = FinancialTransactionTypeEnum.FinancialTransactionTypeDebit;
                    break;
                case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumCredit:
                    spTransaction.TransactionType = FinancialTransactionTypeEnum.FinancialTransactionTypeCredit;
                    break;
                case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumNegativeDebit:
                    spTransaction.TransactionType = FinancialTransactionTypeEnum.FinancialTransactionTypeDebit;
                    spTransaction.AmountNet = spTransaction.AmountNet * -1;
                    spTransaction.AmountFpa = spTransaction.AmountFpa * -1;
                    spTransaction.AmountDiscount = spTransaction.AmountDiscount * -1;
                    break;
                case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumNegativeCredit:
                    spTransaction.TransactionType = FinancialTransactionTypeEnum.FinancialTransactionTypeCredit;
                    spTransaction.AmountNet = spTransaction.AmountNet * -1;
                    spTransaction.AmountFpa = spTransaction.AmountFpa * -1;
                    spTransaction.AmountDiscount = spTransaction.AmountDiscount * -1;
                    break;
                default:
                    break;
            }
           
           
            _context.CustomerTransactions.Add (spTransaction);
            await _context.SaveChangesAsync();
            toastNotification.AddSuccessToastMessage("Saved");
            return RedirectToPage("./Index");
        }
    }
}