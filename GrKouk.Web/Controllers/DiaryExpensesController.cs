using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos;
using GrKouk.WebApi.Data;

namespace GrKouk.Web.Controllers
{
    public class DiaryExpensesController : Controller
    {
        private readonly ApiDbContext _context;

        public DiaryExpensesController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: DiaryExpenses
        public async Task<IActionResult> Index()
        {
            var apiDbContext = _context.FinDiaryTransactions
                .Include(f => f.Company)
                .Include(f => f.CostCentre)
                .Include(f => f.FinTransCategory)
               // .Include(f => f.RevenueCentre)
                .Include(f => f.Transactor);

            var sendModel = Mapper.Map<IEnumerable<FinDiaryExpenseTransactionDto>>(await apiDbContext.ToListAsync());
            return View(sendModel);
        }

        // GET: DiaryExpenses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var finDiaryTransaction = await _context.FinDiaryTransactions
                .Include(f => f.Company)
                .Include(f => f.CostCentre)
                .Include(f => f.FinTransCategory)
                //.Include(f => f.RevenueCentre)
                .Include(f => f.Transactor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (finDiaryTransaction == null)
            {
                return NotFound();
            }

            return View(finDiaryTransaction);
        }

        // GET: DiaryExpenses/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Code");
            ViewData["CostCentreId"] = new SelectList(_context.CostCentres, "Id", "Code");
            ViewData["FinTransCategoryId"] = new SelectList(_context.FinTransCategories, "Id", "Code");
            ViewData["RevenueCentreId"] = new SelectList(_context.RevenueCentres, "Id", "Code");
            ViewData["TransactorId"] = new SelectList(_context.Transactors, "Id", "Id");
            return View();
        }

        // POST: DiaryExpenses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TransactionDate,ReferenceCode,TransactorId,FinTransCategoryId,CompanyId,CostCentreId,RevenueCentreId,Description,Kind,AmountFpa,AmountNet,Timestamp")] FinDiaryTransaction finDiaryTransaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(finDiaryTransaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Code", finDiaryTransaction.CompanyId);
            ViewData["CostCentreId"] = new SelectList(_context.CostCentres, "Id", "Code", finDiaryTransaction.CostCentreId);
            ViewData["FinTransCategoryId"] = new SelectList(_context.FinTransCategories, "Id", "Code", finDiaryTransaction.FinTransCategoryId);
            ViewData["RevenueCentreId"] = new SelectList(_context.RevenueCentres, "Id", "Code", finDiaryTransaction.RevenueCentreId);
            ViewData["TransactorId"] = new SelectList(_context.Transactors, "Id", "Id", finDiaryTransaction.TransactorId);
            return View(finDiaryTransaction);
        }

        // GET: DiaryExpenses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var finDiaryTransaction = await _context.FinDiaryTransactions.FindAsync(id);
            if (finDiaryTransaction == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Code", finDiaryTransaction.CompanyId);
            ViewData["CostCentreId"] = new SelectList(_context.CostCentres, "Id", "Code", finDiaryTransaction.CostCentreId);
            ViewData["FinTransCategoryId"] = new SelectList(_context.FinTransCategories, "Id", "Code", finDiaryTransaction.FinTransCategoryId);
            ViewData["RevenueCentreId"] = new SelectList(_context.RevenueCentres, "Id", "Code", finDiaryTransaction.RevenueCentreId);
            ViewData["TransactorId"] = new SelectList(_context.Transactors, "Id", "Id", finDiaryTransaction.TransactorId);
            return View(finDiaryTransaction);
        }

        // POST: DiaryExpenses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TransactionDate,ReferenceCode,TransactorId,FinTransCategoryId,CompanyId,CostCentreId,RevenueCentreId,Description,Kind,AmountFpa,AmountNet,Timestamp")] FinDiaryTransaction finDiaryTransaction)
        {
            if (id != finDiaryTransaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(finDiaryTransaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FinDiaryTransactionExists(finDiaryTransaction.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Code", finDiaryTransaction.CompanyId);
            ViewData["CostCentreId"] = new SelectList(_context.CostCentres, "Id", "Code", finDiaryTransaction.CostCentreId);
            ViewData["FinTransCategoryId"] = new SelectList(_context.FinTransCategories, "Id", "Code", finDiaryTransaction.FinTransCategoryId);
            ViewData["RevenueCentreId"] = new SelectList(_context.RevenueCentres, "Id", "Code", finDiaryTransaction.RevenueCentreId);
            ViewData["TransactorId"] = new SelectList(_context.Transactors, "Id", "Id", finDiaryTransaction.TransactorId);
            return View(finDiaryTransaction);
        }

        // GET: DiaryExpenses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var finDiaryTransaction = await _context.FinDiaryTransactions
                .Include(f => f.Company)
                .Include(f => f.CostCentre)
                .Include(f => f.FinTransCategory)
                .Include(f => f.RevenueCentre)
                .Include(f => f.Transactor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (finDiaryTransaction == null)
            {
                return NotFound();
            }

            return View(finDiaryTransaction);
        }

        // POST: DiaryExpenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var finDiaryTransaction = await _context.FinDiaryTransactions.FindAsync(id);
            _context.FinDiaryTransactions.Remove(finDiaryTransaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FinDiaryTransactionExists(int id)
        {
            return _context.FinDiaryTransactions.Any(e => e.Id == id);
        }
    }
}
