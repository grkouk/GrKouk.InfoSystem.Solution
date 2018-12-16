using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos.WebDtos.SupplierTransactions;
using AutoMapper;
using NToastNotify;
using Microsoft.EntityFrameworkCore;

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
            
            LoadCompbos();
            return Page();
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
       
        [BindProperty]
        public SupplierTransactionCreateDto SupplierTransactionDto { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                LoadCompbos();
                return Page();
                //return RedirectToPage("./create");
            }

            var spTransaction = mapper.Map<SupplierTransaction>(SupplierTransactionDto);

            var docSeries = _context.TransSupplierDocSeriesDefs.SingleOrDefault(m => m.Id == SupplierTransactionDto.TransSupplierDocSeriesId);

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
            _context.Entry(transSupplierDef)
                .Reference(t => t.CreditTrans)
                .Load();

            _context.Entry(transSupplierDef)
                .Reference(t => t.DebitTrans)
                .Load();
            var creditTrans = transSupplierDef.CreditTrans;
            var debitTrans = transSupplierDef.DebitTrans;

            var section =  _context.Sections.SingleOrDefault(s => s.SystemName == _supplierTransSectionCode);
            if (section == null)
            {

                ModelState.AddModelError(string.Empty, "Δεν υπάρχει το Section");
                LoadCompbos();
                return Page();
            }

            spTransaction.SectionId = section.Id;
            spTransaction.TransSupplierDocTypeId = docSeries.TransSupplierDocTypeDefId;
            spTransaction.FiscalPeriodId = 1;

            if (creditTrans.Action == "=" && debitTrans.Action != "=")
            {
                spTransaction.TransactionType = InfoSystem.Domain.FinConfig.FinancialTransactionTypeEnum.FinancialTransactionTypeDebit;
                switch (debitTrans.Action)
                {
                    case "+":

                        break;
                    case "-":
                        spTransaction.AmountNet = spTransaction.AmountNet * -1;
                        spTransaction.AmountFpa = spTransaction.AmountFpa * -1;
                        spTransaction.AmountDiscount = spTransaction.AmountDiscount * -1;
                        break;
                }
            }
            else if (creditTrans.Action != "=" && debitTrans.Action == "=")
            {
                spTransaction.TransactionType = InfoSystem.Domain.FinConfig.FinancialTransactionTypeEnum.FinancialTransactionTypeCredit;
                switch (creditTrans.Action)
                {
                    case "+":

                        break;
                    case "-":
                        spTransaction.AmountNet = spTransaction.AmountNet * -1;
                        spTransaction.AmountFpa = spTransaction.AmountFpa * -1;
                        spTransaction.AmountDiscount = spTransaction.AmountDiscount * -1;
                        break;
                }
            }
            _context.SupplierTransactions.Add(spTransaction);
            await _context.SaveChangesAsync();
            toastNotification.AddSuccessToastMessage("Saved");
            return RedirectToPage("./Index");
        }
    }
}