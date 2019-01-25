﻿using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GrKouk.InfoSystem.Dtos.WebDtos.BuyMaterialsDocs;
using GrKouk.InfoSystem.Dtos.WebDtos.SellDocuments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace GrKouk.WebRazor.Pages.Transactions.SellMaterialDoc
{
    public class EditModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;
        public bool InitialLoad = true;

        public EditModel(GrKouk.WebApi.Data.ApiDbContext context, IMapper mapper, IToastNotification toastNotification)
        {
            _context = context;
            _mapper = mapper;
            _toastNotification = toastNotification;
        }

        [BindProperty]
        public SellDocModifyDto ItemVm { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var sellMatDoc = await _context.SellDocuments
                .Include(b => b.Company)
                .Include(b => b.FiscalPeriod)
                .Include(b => b.SellDocSeries)
                .Include(b => b.SellDocType)
                .Include(b => b.Section)

                .Include(b => b.Transactor)
                .Include(b => b.SellDocLines)
                .ThenInclude(m => m.Material)
                .FirstOrDefaultAsync(m => m.Id == id);

            ItemVm = _mapper.Map<SellDocModifyDto>(sellMatDoc);

            if (ItemVm == null)
            {
                return NotFound();
            }

          

            LoadCombos();
            return Page();
        }

        private void LoadCombos()
        {
            var transactorList = _context.Transactors.Where(s => s.TransactorType.Code == "SYS.CUSTOMER").OrderBy(s => s.Name).AsNoTracking();
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(p => p.Code).AsNoTracking(), "Id", "Code");
            ViewData["SellDocSeriesId"] = new SelectList(_context.SellDocSeriesDefs.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
            ViewData["TransactorId"] = new SelectList(transactorList, "Id", "Name");
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(ItemVm).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BuyMaterialsDocumentExists(ItemVm.Id))
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

        private bool BuyMaterialsDocumentExists(int id)
        {
            return _context.BuyMaterialsDocuments.Any(e => e.Id == id);
        }
    }
}