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
            //If section is not our section the cannot update disable input controls
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
            if (ItemVm.FiscalPeriodId <= 0)
            {
                ModelState.AddModelError(string.Empty, "No Fiscal Period covers Transaction Date");
                LoadCombos();
                return Page();
            }
            var transToAttach = _mapper.Map<WarehouseTransaction>(ItemVm);
            var docSeries = await
                _context.TransWarehouseDocSeriesDefs.SingleOrDefaultAsync(m =>
                    m.Id == ItemVm.TransWarehouseDocSeriesId);

            if (docSeries is null)
            {
                ModelState.AddModelError(string.Empty, "Δεν βρέθηκε η σειρά παραστατικού");
                LoadCombos();
                return Page();
            }
            await _context.Entry(docSeries).Reference(t => t.TransWarehouseDocTypeDef).LoadAsync();

            var docTypeDef = docSeries.TransWarehouseDocTypeDef;
            await _context.Entry(docTypeDef)
                .Reference(t => t.TransWarehouseDef)
                .LoadAsync();

            var transWarehouseDef = docTypeDef.TransWarehouseDef;
            transToAttach.TransWarehouseDocTypeId = docSeries.TransWarehouseDocTypeDefId;
            transToAttach.InventoryAction = transWarehouseDef.InventoryTransType;
            switch (transWarehouseDef.InventoryTransType)
            {
                case WarehouseInventoryTransTypeEnum.WarehouseInventoryTransTypeEnumNoChange:
                    break;
                case WarehouseInventoryTransTypeEnum.WarehouseInventoryTransTypeEnumImport:

                    transToAttach.TransactionType =
                        WarehouseTransactionTypeEnum.WarehouseTransactionTypeImport;
                    //transToAttach.Quontity1 = dataBuyDocLine.Q1;
                    //transToAttach.Quontity2 = dataBuyDocLine.Q2;
                    break;
                case WarehouseInventoryTransTypeEnum.WarehouseInventoryTransTypeEnumExport:

                    transToAttach.TransactionType =
                        WarehouseTransactionTypeEnum.WarehouseTransactionTypeExport;
                    //transToAttach.Quontity1 = dataBuyDocLine.Q1;
                    //transToAttach.Quontity2 = dataBuyDocLine.Q2;
                    break;
                case WarehouseInventoryTransTypeEnum.WarehouseInventoryTransTypeEnumNegativeImport:

                    transToAttach.TransactionType =
                        WarehouseTransactionTypeEnum.WarehouseTransactionTypeImport;
                    transToAttach.Quontity1 = transToAttach.Quontity1 * -1;
                    transToAttach.Quontity2 = transToAttach.Quontity2 * -1;
                    break;
                case WarehouseInventoryTransTypeEnum.WarehouseInventoryTransTypeEnumNegativeExport:

                    transToAttach.TransactionType =
                        WarehouseTransactionTypeEnum.WarehouseTransactionTypeExport;
                    transToAttach.Quontity1 = transToAttach.Quontity1 * -1;
                    transToAttach.Quontity2 = transToAttach.Quontity2 * -1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            transToAttach.InventoryValueAction = transWarehouseDef.InventoryValueTransType;

            switch (transWarehouseDef.InventoryValueTransType)
            {
                case WarehouseValueTransTypeEnum.WarehouseValueTransTypeEnumNoChange:
                    break;
                case WarehouseValueTransTypeEnum.WarehouseValueTransTypeEnumIncrease:

                    break;
                case WarehouseValueTransTypeEnum.WarehouseValueTransTypeEnumDecrease:

                    break;
                case WarehouseValueTransTypeEnum.WarehouseValueTransTypeEnumNegativeIncrease:
                    transToAttach.AmountNet = transToAttach.AmountNet * -1;
                    transToAttach.AmountDiscount = transToAttach.AmountDiscount * -1;
                    transToAttach.AmountFpa = transToAttach.AmountFpa * -1;
                    break;
                case WarehouseValueTransTypeEnum.WarehouseValueTransTypeEnumNegativeDecrease:
                    transToAttach.AmountNet = transToAttach.AmountNet * -1;
                    transToAttach.AmountDiscount = transToAttach.AmountDiscount * -1;
                    transToAttach.AmountFpa = transToAttach.AmountFpa * -1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _context.Attach(transToAttach).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Saved");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WarehouseTransactionExists(transToAttach.Id))
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

        private bool WarehouseTransactionExists(int id)
        {
            return _context.WarehouseTransactions.Any(e => e.Id == id);
        }
    }
}
