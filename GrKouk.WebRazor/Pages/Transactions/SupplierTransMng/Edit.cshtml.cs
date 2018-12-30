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
        public EditModel(GrKouk.WebApi.Data.ApiDbContext context, IMapper mapper, IToastNotification toastNotification)
        {
            _context = context;
            _mapper = mapper;
            _toastNotification = toastNotification;
        }

        [BindProperty]
        public SupplierTransactionModifyDto SupplierTransactionVm { get; set; }

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

            SupplierTransactionVm = _mapper.Map<SupplierTransactionModifyDto>(supplierTransactionToModify);
            LoadCompbos();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                LoadCompbos();
                return Page();
            }

            var spTransactionToAttach = _mapper.Map<SupplierTransaction>(SupplierTransactionVm);

            var docSeries = _context.TransSupplierDocSeriesDefs.SingleOrDefault(m => m.Id == spTransactionToAttach.TransSupplierDocSeriesId);

            if (docSeries is null)
            {
                ModelState.AddModelError(string.Empty, "Δεν βρέθηκε η σειρά παραστατικού");
                LoadCompbos();
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
            // spTransaction.FiscalPeriodId = 1;
            switch (transSupplierDef.FinancialTransType)
            {
                case InfoSystem.Domain.FinConfig.FinancialTransTypeEnum.FinancialTransTypeNoChange:
                    break;
                case InfoSystem.Domain.FinConfig.FinancialTransTypeEnum.FinancialTransTypeDebit:
                    spTransactionToAttach.FinancialAction = FinancialTransTypeEnum.FinancialTransTypeDebit;
                    spTransactionToAttach.TransactionType = FinancialTransactionTypeDebit;
                    break;
                case InfoSystem.Domain.FinConfig.FinancialTransTypeEnum.FinancialTransTypeCredit:
                    spTransactionToAttach.FinancialAction = FinancialTransTypeEnum.FinancialTransTypeCredit;
                    spTransactionToAttach.TransactionType = FinancialTransactionTypeCredit;
                    break;
                case InfoSystem.Domain.FinConfig.FinancialTransTypeEnum.FinancialTransTypeNegativeDebit:
                    spTransactionToAttach.FinancialAction = FinancialTransTypeEnum.FinancialTransTypeNegativeDebit;
                    spTransactionToAttach.TransactionType = FinancialTransactionTypeDebit;
                    spTransactionToAttach.AmountNet = spTransactionToAttach.AmountNet * -1;
                    spTransactionToAttach.AmountFpa = spTransactionToAttach.AmountFpa * -1;
                    spTransactionToAttach.AmountDiscount = spTransactionToAttach.AmountDiscount * -1;
                    break;
                case InfoSystem.Domain.FinConfig.FinancialTransTypeEnum.FinancialTransTypeNegativeCredit:
                    spTransactionToAttach.FinancialAction = FinancialTransTypeEnum.FinancialTransTypeNegativeCredit;
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
                if (!SupplierTransactionExists(SupplierTransactionVm.Id))
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
        private void LoadCompbos()
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
