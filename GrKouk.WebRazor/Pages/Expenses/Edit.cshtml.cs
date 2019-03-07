﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GrKouk.InfoSystem.Domain.FinConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos;
using GrKouk.WebApi.Data;
using NToastNotify;

namespace GrKouk.WebRazor.Pages.Expenses
{
    public class EditModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;


        public EditModel(GrKouk.WebApi.Data.ApiDbContext context, IMapper mapper, IToastNotification toastNotification)
        {
            _context = context;
            _mapper = mapper;
            _toastNotification = toastNotification;
        }

        [BindProperty]
        public FinDiaryExpenceTransModifyDto FinDiaryTransactionVM { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var diaryTransactionToModify = await _context.FinDiaryTransactions
                .Include(f => f.Company)
                .Include(f => f.CostCentre)
                .Include(f => f.FinTransCategory)
                .Include(f => f.RevenueCentre)
                .Include(f => f.Transactor).FirstOrDefaultAsync(m => m.Id == id);

            if (diaryTransactionToModify == null)
            {
                return NotFound();
            }

            FinDiaryTransactionVM = _mapper.Map<FinDiaryExpenceTransModifyDto>(diaryTransactionToModify);

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

            var diaryTransactionToAttach = _mapper.Map<FinDiaryTransaction>(FinDiaryTransactionVM);

            //diaryTransactionToAttach.Kind = (int)DiaryTransactionsKindEnum.Expence;
            //diaryTransactionToAttach.RevenueCentreId = 1;

            _context.Attach(diaryTransactionToAttach).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FinDiaryTransactionExists(diaryTransactionToAttach.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return RedirectToPage("./Index3");
        }

        private bool FinDiaryTransactionExists(int id)
        {
            return _context.FinDiaryTransactions.Any(e => e.Id == id);
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
