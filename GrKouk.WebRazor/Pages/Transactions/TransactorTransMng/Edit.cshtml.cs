﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GrKouk.InfoSystem.Definitions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos.WebDtos.TransactorTransactions;
using Microsoft.AspNetCore.Authorization;
using NToastNotify;

namespace GrKouk.WebRazor.Pages.Transactions.TransactorTransMng
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private const string _sectionCode = "SYS-TRANSACTOR-TRANS";
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
        public TransactorTransModifyDto ItemVm { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactionToModify = await _context.TransactorTransactions
                .Include(t => t.Company)
                .Include(t => t.FiscalPeriod)
                .Include(t => t.Section)
                .Include(t => t.TransTransactorDocSeries)
                .Include(t => t.TransTransactorDocType)
                .Include(t => t.Transactor).FirstOrDefaultAsync(m => m.Id == id);

            if (transactionToModify == null)
            {
                return NotFound();
            }

            var section = _context.Sections.SingleOrDefault(s => s.SystemName == _sectionCode);
            if (section is null)
            {
                _toastNotification.AddAlertToastMessage("Supplier Transactions section not found in DB");
                return BadRequest();
            }
            //If section is not our section the canot update disable input controls
            NotUpdatable = transactionToModify.SectionId != section.Id;

            ItemVm = _mapper.Map<TransactorTransModifyDto>(transactionToModify);
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
            var spTransactionToAttach = _mapper.Map<TransactorTransaction>(ItemVm);
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
            var docSeries = _context.TransTransactorDocSeriesDefs.SingleOrDefault(m => m.Id == spTransactionToAttach.TransTransactorDocSeriesId);

            if (docSeries is null)
            {
                ModelState.AddModelError(string.Empty, "Δεν βρέθηκε η σειρά παραστατικού");
                LoadCombos();
                return Page();
            }
            _context.Entry(docSeries).Reference(t => t.TransTransactorDocTypeDef).Load();

            var docTypeDef = docSeries.TransTransactorDocTypeDef;
            _context.Entry(docTypeDef)
                .Reference(t => t.TransTransactorDef)
                .Load();
            var transTransactorDef = docTypeDef.TransTransactorDef;


            //spTransaction.SectionId = section.Id;
            spTransactionToAttach.TransTransactorDocTypeId = docSeries.TransTransactorDocTypeDefId;
            spTransactionToAttach.FinancialAction = transTransactorDef.FinancialTransAction;
            switch (transTransactorDef.FinancialTransAction)
            {
                case FinActionsEnum.FinActionsEnumNoChange:
                    spTransactionToAttach.TransDiscountAmount = 0;
                    spTransactionToAttach.TransFpaAmount = 0;
                    spTransactionToAttach.TransNetAmount = 0;
                    break;
                case FinActionsEnum.FinActionsEnumDebit:
                    spTransactionToAttach.TransDiscountAmount = spTransactionToAttach.AmountDiscount;
                    spTransactionToAttach.TransFpaAmount = spTransactionToAttach.AmountFpa;
                    spTransactionToAttach.TransNetAmount = spTransactionToAttach.AmountNet;
                    break;
                case FinActionsEnum.FinActionsEnumCredit:
                    spTransactionToAttach.TransDiscountAmount = spTransactionToAttach.AmountDiscount;
                    spTransactionToAttach.TransFpaAmount = spTransactionToAttach.AmountFpa;
                    spTransactionToAttach.TransNetAmount = spTransactionToAttach.AmountNet;
                    break;
                case FinActionsEnum.FinActionsEnumNegativeDebit:
                    spTransactionToAttach.TransNetAmount = spTransactionToAttach.AmountNet * -1;
                    spTransactionToAttach.TransFpaAmount = spTransactionToAttach.AmountFpa * -1;
                    spTransactionToAttach.TransDiscountAmount = spTransactionToAttach.AmountDiscount * -1;
                    break;
                case FinActionsEnum.FinActionsEnumNegativeCredit:
                    spTransactionToAttach.TransNetAmount = spTransactionToAttach.AmountNet * -1;
                    spTransactionToAttach.TransFpaAmount = spTransactionToAttach.AmountFpa * -1;
                    spTransactionToAttach.TransDiscountAmount = spTransactionToAttach.AmountDiscount * -1;
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
                if (!TransactorTransactionExists(ItemVm.Id))
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

        private bool TransactorTransactionExists(int id)
        {
            return _context.TransactorTransactions.Any(e => e.Id == id);
        }
        private void LoadCombos()
        {
            var transactorsListDb = _context.Transactors
                .Include(p => p.TransactorType)
                .Where(p => p.TransactorType.Code != "SYS.DTRANSACTOR")
                .OrderBy(s => s.Name).AsNoTracking();
            List<SelectListItem> transactorsList = new List<SelectListItem>();

            foreach (var dbTransactor in transactorsListDb)
            {

                transactorsList.Add(new SelectListItem() { Value = dbTransactor.Id.ToString(), Text = dbTransactor.Name + "-" + dbTransactor.TransactorType.Code });
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(c => c.Code).AsNoTracking(), "Id", "Code");
            ViewData["FiscalPeriodId"] = new SelectList(_context.FiscalPeriods.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
            ViewData["TransactorId"] = new SelectList(transactorsList, "Value", "Text");
            ViewData["TransTransactorDocSeriesId"] = new SelectList(_context.TransTransactorDocSeriesDefs.OrderBy(s => s.Name).AsNoTracking(), "Id", "Name");
            //ViewData["SectionId"] = new SelectList(_context.Sections, "Id", "Code");
        }
    }
}
