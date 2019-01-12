using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos.WebDtos.CustomerTransactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace GrKouk.WebRazor.Pages.Transactions.CustomerTransMng
{
    public class EditModel : PageModel
    {
        private const string SupplierTransSectionCode = "SYS-CUSTOMER-TRANS";

        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;
        public bool NotUpdatable;
        public bool InitialLoad = true;
        public EditModel(GrKouk.WebApi.Data.ApiDbContext context, IMapper mapper, IToastNotification toastNotification)
        {
            _context = context;
            _mapper = mapper;
            _toastNotification = toastNotification;
        }

        [BindProperty]
        public CustomerTransactionModifyDto ItemVm { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

           var  customerTransactionToModify = await _context.CustomerTransactions
                .Include(s => s.Company)
                .Include(s => s.FiscalPeriod)
              
                .Include(s => s.Section)
                .Include(s => s.Customer)
                .Include(s => s.TransCustomerDocSeries)
                .Include(s => s.TransCustomerDocType).FirstOrDefaultAsync(m => m.Id == id);

            if (customerTransactionToModify == null)
            {
                return NotFound();
            }
            var section = _context.Sections.SingleOrDefault(s => s.SystemName == SupplierTransSectionCode);
            if (section is null)
            {
                _toastNotification.AddAlertToastMessage("Customer Transactions section not found in DB");
                return BadRequest();
            }
            //If section is not our section the canot update disable input controls
            NotUpdatable = customerTransactionToModify.SectionId != section.Id;

            ItemVm = _mapper.Map<CustomerTransactionModifyDto>(customerTransactionToModify);
            LoadCombos();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                LoadCombos();
                return Page();
            }
            if (ItemVm.FiscalPeriodId <= 0)
            {
                ModelState.AddModelError(string.Empty, "No Fiscal Period covers Transaction Date");
                LoadCombos();
                return Page();
            }
            var spTransactionToAttach = _mapper.Map<CustomerTransaction>(ItemVm);
           
            var docSeries = _context.TransCustomerDocSeriesDefs.SingleOrDefault(m => m.Id == spTransactionToAttach.TransCustomerDocSeriesId);

            if (docSeries is null)
            {
                ModelState.AddModelError(string.Empty, "Δεν βρέθηκε η σειρά παραστατικού");
                LoadCombos();
                return Page();
            }
            _context.Entry(docSeries).Reference(t => t.TransCustomerDocTypeDef).Load();

            var docTypeDef = docSeries.TransCustomerDocTypeDef;
            _context.Entry(docTypeDef)
                .Reference(t => t.TransCustomerDef)
                .Load();
            var transCustomerDef = docTypeDef.TransCustomerDef;
           

            //spTransaction.SectionId = section.Id;
            spTransactionToAttach.TransCustomerDocTypeId = docSeries.TransCustomerDocTypeDefId;
            spTransactionToAttach.FinancialAction = transCustomerDef.FinancialTransAction;
            switch (transCustomerDef.FinancialTransAction)
            {
                case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumNoChange:
                    spTransactionToAttach.TransactionType = FinancialTransactionTypeEnum.FinancialTransactionTypeIgnore;
                    break;
                case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumDebit:
                    spTransactionToAttach.TransactionType = FinancialTransactionTypeEnum.FinancialTransactionTypeDebit;
                    break;
                case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumCredit:
                    spTransactionToAttach.TransactionType = FinancialTransactionTypeEnum.FinancialTransactionTypeCredit;
                    break;
                case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumNegativeDebit:
                    spTransactionToAttach.TransactionType = FinancialTransactionTypeEnum.FinancialTransactionTypeDebit;
                    spTransactionToAttach.AmountNet = spTransactionToAttach.AmountNet * -1;
                    spTransactionToAttach.AmountFpa = spTransactionToAttach.AmountFpa * -1;
                    spTransactionToAttach.AmountDiscount = spTransactionToAttach.AmountDiscount * -1;
                    break;
                case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumNegativeCredit:
                    spTransactionToAttach.TransactionType = FinancialTransactionTypeEnum.FinancialTransactionTypeCredit;
                    spTransactionToAttach.AmountNet = spTransactionToAttach.AmountNet * -1;
                    spTransactionToAttach.AmountFpa = spTransactionToAttach.AmountFpa * -1;
                    spTransactionToAttach.AmountDiscount = spTransactionToAttach.AmountDiscount * -1;
                    break;
                default:
                    break;
            }
           

            _context.Attach(spTransactionToAttach).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerTransactionExists(ItemVm.Id))
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

        private bool CustomerTransactionExists(int id)
        {
            return _context.CustomerTransactions.Any(e => e.Id == id);
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
    }
}
