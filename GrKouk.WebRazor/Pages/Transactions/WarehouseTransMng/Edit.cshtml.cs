using System;
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
using GrKouk.InfoSystem.Dtos.WebDtos.WarehouseTransactions;
using GrKouk.WebApi.Data;
using NToastNotify;

namespace GrKouk.WebRazor.Pages.Transactions.WarehouseTransMng
{
    public class EditModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;
        public bool NotUpdatable;
        public bool InitialLoad = true;

        private const string SectionSystemCode = "SYS-WAREHOUSE-TRANS";

        public EditModel(GrKouk.WebApi.Data.ApiDbContext context, IMapper mapper, IToastNotification toastNotification)
        {
            _context = context;
            _mapper = mapper;
            _toastNotification = toastNotification;
        }

        [BindProperty]
        public WarehouseTransModifyDto ItemVm { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transToEdit = await _context.WarehouseTransactions
                .Include(w => w.Company)
                .Include(w => w.FiscalPeriod)
                .Include(w => w.Material).ThenInclude(m=>m.MainMeasureUnit)
                .Include(w => w.Material).ThenInclude(m => m.SecondaryMeasureUnit)
                .Include(w => w.Section)
                .Include(w => w.TransWarehouseDocSeries)
                .Include(w => w.TransWarehouseDocType).FirstOrDefaultAsync(m => m.Id == id);

            if (transToEdit == null)
            {
                return NotFound();
            }
            var section = _context.Sections.SingleOrDefault(s => s.SystemName == SectionSystemCode);
            if (section is null)
            {
                _toastNotification.AddAlertToastMessage("Warehouse Transactions section not found in DB");
                return BadRequest();
            }
            //If section is not our section the canot update disable input controls
            NotUpdatable = transToEdit.SectionId != section.Id;

            ItemVm = _mapper.Map<WarehouseTransModifyDto>(transToEdit);
            ItemVm.PrimaryUnitCode = transToEdit.Material.MainMeasureUnit.Code;
            ItemVm.SecondaryUnitCode = transToEdit.Material.SecondaryMeasureUnit.Code;
            LoadCombos();
           return Page();
        }

        private void LoadCombos()
        {
            List<SelectListItem> warTransTypes = new List<SelectListItem>
            {
                new SelectListItem() {Value = WarehouseTransactionTypeEnum.WarehouseTransactionTypeImport.ToString(), Text = "Import"},
                new SelectListItem() {Value = WarehouseTransactionTypeEnum.WarehouseTransactionTypeExport.ToString(), Text = "Export"},

            };
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(p => p.Code).AsNoTracking(), "Id", "Code");
            ViewData["FiscalPeriodId"] = new SelectList(_context.FiscalPeriods.OrderBy(p=>p.Name).AsNoTracking(), "Id", "Name");
            ViewData["MaterialId"] = new SelectList(_context.Materials.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
            //ViewData["SectionId"] = new SelectList(_context.Sections, "Id", "Code");
            ViewData["TransWarehouseDocSeriesId"] = new SelectList(_context.TransWarehouseDocSeriesDefs.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
            ViewData["TransactionType"] = new SelectList(warTransTypes, "Value", "Text");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //_context.Attach(WarehouseTransaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //if (!WarehouseTransactionExists(WarehouseTransaction.Id))
                //{
                //    return NotFound();
                //}
                //else
                //{
                //    throw;
                //}
            }

            return RedirectToPage("./Index");
        }

        private bool WarehouseTransactionExists(int id)
        {
            return _context.WarehouseTransactions.Any(e => e.Id == id);
        }
    }
}
