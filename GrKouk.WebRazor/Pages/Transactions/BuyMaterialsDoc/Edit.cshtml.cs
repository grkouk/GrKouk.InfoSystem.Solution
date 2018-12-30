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

        public EditModel(GrKouk.WebApi.Data.ApiDbContext context,IMapper mapper,IToastNotification toastNotification)
        {
            _context = context;
            _mapper = mapper;
            _toastNotification = toastNotification;
        }

        [BindProperty]
        public BuyMaterialsDocument ItemVm { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ItemVm = await _context.BuyMaterialsDocuments
                .Include(b => b.Company)
                .Include(b => b.FiscalPeriod)
                .Include(b => b.MaterialDocSeries)
                .Include(b => b.MaterialDocType)
                .Include(b => b.Section)
                .Include(b => b.Supplier).FirstOrDefaultAsync(m => m.Id == id);

            if (ItemVm == null)
            {
                return NotFound();
            }
           LoadCombos();
           return Page();
        }

        private void LoadCombos()
        {
            var supplierList = _context.Transactors.Where(s => s.TransactorType.Code == "SYS.SUPPLIER").OrderBy(s => s.Name).AsNoTracking();
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Code");
            ViewData["FiscalPeriodId"] = new SelectList(_context.FiscalPeriods, "Id", "Id");
            ViewData["MaterialDocSeriesId"] = new SelectList(_context.BuyMaterialDocSeriesDefs, "Id", "Code");
            ViewData["MaterialDocTypeId"] = new SelectList(_context.BuyMaterialDocTypeDefs, "Id", "Code");
            ViewData["SectionId"] = new SelectList(_context.Sections, "Id", "Code");
            ViewData["SupplierId"] = new SelectList(supplierList, "Id", "Name");
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
