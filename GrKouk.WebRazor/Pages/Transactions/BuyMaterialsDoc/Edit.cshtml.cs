using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos.WebDtos.BuyDocuments;
using GrKouk.WebApi.Data;
using NToastNotify;

namespace GrKouk.WebRazor.Pages.Transactions.BuyMaterialsDoc
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
        public BuyDocModifyDto ItemVm { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var buyMatDoc = await _context.BuyDocuments
                .Include(b => b.Company)
                .Include(b => b.FiscalPeriod)
                .Include(b => b.BuyDocSeries)
                .Include(b => b.BuyDocType)
                .Include(b => b.Section)

                .Include(b => b.Transactor)
                .Include(b => b.BuyDocLines)
                .ThenInclude(m => m.Material)
                .FirstOrDefaultAsync(m => m.Id == id);

            ItemVm = _mapper.Map<BuyDocModifyDto>(buyMatDoc);

            if (ItemVm == null)
            {
                return NotFound();
            }

            //ItemVm.AmountNet = buyMatDoc.BuyDocLines.Sum(p => p.AmountNet);
            //ItemVm.AmountFpa = buyMatDoc.BuyDocLines.Sum(p => p.AmountFpa);
            //ItemVm.AmountDiscount = buyMatDoc.BuyDocLines.Sum(p => p.AmountDiscount);

            LoadCombos();
            return Page();
        }

        private void LoadCombos()
        {
            var supplierList = _context.Transactors.Where(s => s.TransactorType.Code == "SYS.SUPPLIER").OrderBy(s => s.Name).AsNoTracking();
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(p => p.Code).AsNoTracking(), "Id", "Code");
            ViewData["BuyDocSeriesId"] = new SelectList(_context.BuyDocSeriesDefs.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
            ViewData["TransactorId"] = new SelectList(supplierList, "Id", "Name");
           
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
            return _context.BuyDocuments.Any(e => e.Id == id);
        }
    }
}
