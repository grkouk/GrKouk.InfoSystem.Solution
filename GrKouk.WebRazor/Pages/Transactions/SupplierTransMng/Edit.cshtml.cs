using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GrKouk.InfoSystem.Domain.FinConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos.WebDtos.SupplierTransactions;
using NToastNotify;
using static GrKouk.InfoSystem.Domain.FinConfig.FinancialTransactionTypeEnum;

namespace GrKouk.WebRazor.Pages.Transactions.SupplierTransMng
{
    public class EditModel : PageModel
    {
        private const string SupplierTransSectionCode = "SYS-SUPPLIER-TRANS";

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
        public SupplierTransactionModifyDto ItemVm { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

           var  supplierTransactionToModify = await _context.SupplierTransactions
                .Include(s => s.Company)
                .Include(s => s.FiscalPeriod)
              
                .Include(s => s.Section)
                .Include(s => s.Supplier)
                .Include(s => s.TransSupplierDocSeries)
                .Include(s => s.TransSupplierDocType).FirstOrDefaultAsync(m => m.Id == id);

            if (supplierTransactionToModify == null)
            {
                return NotFound();
            }
            var section = _context.Sections.SingleOrDefault(s => s.SystemName == SupplierTransSectionCode);
            if (section is null)
            {
                _toastNotification.AddAlertToastMessage("Supplier Transactions section not found in DB");
                return BadRequest();
            }
            //If section is not our section the canot update disable input controls
            NotUpdatable = supplierTransactionToModify.SectionId != section.Id;

            ItemVm = _mapper.Map<SupplierTransactionModifyDto>(supplierTransactionToModify);
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
            var spTransactionToAttach = _mapper.Map<SupplierTransaction>(ItemVm);
            #region Fiscal Period
            //var dateOfTrans = ItemVm.TransDate;
            //var fiscalPeriod = await _context.FiscalPeriods.FirstOrDefaultAsync(p =>
            //    p.StartDate.CompareTo(dateOfTrans) > 0 & p.EndDate.CompareTo(dateOfTrans) < 0);
            //if (fiscalPeriod == null)
            //{
            //    ModelState.AddModelError(string.Empty, "No Fiscal Period covers Transaction Date");
            //    LoadCombos();
            //    return Page();
            //}
            #endregion
            var docSeries = _context.TransSupplierDocSeriesDefs.SingleOrDefault(m => m.Id == spTransactionToAttach.TransSupplierDocSeriesId);

            if (docSeries is null)
            {
                ModelState.AddModelError(string.Empty, "Δεν βρέθηκε η σειρά παραστατικού");
                LoadCombos();
                return Page();
            }
            _context.Entry(docSeries).Reference(t => t.TransSupplierDocTypeDef).Load();

            var docTypeDef = docSeries.TransSupplierDocTypeDef;
            _context.Entry(docTypeDef)
                .Reference(t => t.TransSupplierDef)
                .Load();
            var transSupplierDef = docTypeDef.TransSupplierDef;
           

            //spTransaction.SectionId = section.Id;
            spTransactionToAttach.TransSupplierDocTypeId = docSeries.TransSupplierDocTypeDefId;
            spTransactionToAttach.FinancialAction = transSupplierDef.FinancialAction;
            switch (transSupplierDef.FinancialAction)
            {
                case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumNoChange:
                    spTransactionToAttach.TransactionType = FinancialTransactionTypeIgnore;
                    break;
                case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumDebit:
                    spTransactionToAttach.TransactionType = FinancialTransactionTypeDebit;
                    break;
                case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumCredit:
                    spTransactionToAttach.TransactionType = FinancialTransactionTypeCredit;
                    break;
                case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumNegativeDebit:
                    spTransactionToAttach.TransactionType = FinancialTransactionTypeDebit;
                    spTransactionToAttach.AmountNet = spTransactionToAttach.AmountNet * -1;
                    spTransactionToAttach.AmountFpa = spTransactionToAttach.AmountFpa * -1;
                    spTransactionToAttach.AmountDiscount = spTransactionToAttach.AmountDiscount * -1;
                    break;
                case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumNegativeCredit:
                    spTransactionToAttach.TransactionType = FinancialTransactionTypeCredit;
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
                if (!SupplierTransactionExists(ItemVm.Id))
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

        private bool SupplierTransactionExists(int id)
        {
            return _context.SupplierTransactions.Any(e => e.Id == id);
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
    }
}
