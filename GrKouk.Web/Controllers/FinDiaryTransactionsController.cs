using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos;
using GrKouk.WebApi.Data;
using AutoMapper;

namespace GrKouk.Web.Controllers
{
    public class FinDiaryTransactionsController : Controller
    {
        private readonly ApiDbContext _context;

        public FinDiaryTransactionsController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: FinDiaryTransactions
        public async Task<IActionResult> Index()
        {
            var apiDbContext = _context.FinDiaryTransactions.Include(f => f.Company)
                .Include(f => f.CostCentre)
                .Include(f => f.FinTransCategory)
                .Include(f => f.RevenueCentre)
                .Include(f => f.Transactor);

            var sendModel = Mapper.Map<IEnumerable<FinDiaryTransactionDto>>(await apiDbContext.ToListAsync());
            return View(sendModel);
        }

        // GET: FinDiaryTransactions/Details/5
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
                .Include(f => f.RevenueCentre)
                .Include(f => f.Transactor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (finDiaryTransaction == null)
            {
                return NotFound();
            }

            return View(finDiaryTransaction);
        }

        // GET: FinDiaryTransactions/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(p=>p.Name).AsNoTracking(), "Id", "Name");
            ViewData["CostCentreId"] = new SelectList(_context.CostCentres.OrderBy(p=>p.Name).AsNoTracking(), "Id", "Name");
            ViewData["FinTransCategoryId"] = new SelectList(_context.FinTransCategories.OrderBy(p=>p.Name).AsNoTracking(), "Id", "Name");
            ViewData["RevenueCentreId"] = new SelectList(_context.RevenueCentres.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
            ViewData["TransactorId"] = new SelectList(_context.Transactors.OrderBy(p=>p.Name).AsNoTracking(), "Id", "Name");
            return View();
        }

        // POST: FinDiaryTransactions/Create
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

            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name", finDiaryTransaction.CompanyId);
            ViewData["CostCentreId"] = new SelectList(_context.CostCentres.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name", finDiaryTransaction.CostCentreId);
            ViewData["FinTransCategoryId"] = new SelectList(_context.FinTransCategories.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name", finDiaryTransaction.FinTransCategoryId);
            ViewData["RevenueCentreId"] = new SelectList(_context.RevenueCentres.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name", finDiaryTransaction.RevenueCentreId);
            ViewData["TransactorId"] = new SelectList(_context.Transactors.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name", finDiaryTransaction.TransactorId);

            
            return View(finDiaryTransaction);
        }

        // GET: FinDiaryTransactions/Edit/5
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
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name", finDiaryTransaction.CompanyId);
            ViewData["CostCentreId"] = new SelectList(_context.CostCentres.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name", finDiaryTransaction.CostCentreId);
            ViewData["FinTransCategoryId"] = new SelectList(_context.FinTransCategories.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name", finDiaryTransaction.FinTransCategoryId);
            ViewData["RevenueCentreId"] = new SelectList(_context.RevenueCentres.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name", finDiaryTransaction.RevenueCentreId);
            ViewData["TransactorId"] = new SelectList(_context.Transactors.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name", finDiaryTransaction.TransactorId);

            return View(finDiaryTransaction);
        }

        // POST: FinDiaryTransactions/Edit/5
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
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name", finDiaryTransaction.CompanyId);
            ViewData["CostCentreId"] = new SelectList(_context.CostCentres.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name", finDiaryTransaction.CostCentreId);
            ViewData["FinTransCategoryId"] = new SelectList(_context.FinTransCategories.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name", finDiaryTransaction.FinTransCategoryId);
            ViewData["RevenueCentreId"] = new SelectList(_context.RevenueCentres.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name", finDiaryTransaction.RevenueCentreId);
            ViewData["TransactorId"] = new SelectList(_context.Transactors.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name", finDiaryTransaction.TransactorId);

            return View(finDiaryTransaction);
        }

        // GET: FinDiaryTransactions/Delete/5
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

        // POST: FinDiaryTransactions/Delete/5
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
