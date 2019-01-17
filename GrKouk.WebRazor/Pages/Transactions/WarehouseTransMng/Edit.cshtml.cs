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
            var material = await _context.Materials.SingleOrDefaultAsync(p => p.Id == transToAttach.MaterialId);
            if (material is null)
            {
                ModelState.AddModelError(string.Empty, "Δεν βρέθηκε το είδος");
                LoadCombos();
                return Page();
            }

            switch (material.MaterialNature)
            {
                case MaterialNatureEnum.MaterialNatureEnumUndefined:
                    throw new ArgumentOutOfRangeException();
                    break;
                case MaterialNatureEnum.MaterialNatureEnumMaterial:
                    transToAttach.InventoryAction = transWarehouseDef.MaterialInventoryAction;
                    transToAttach.InventoryValueAction = transWarehouseDef.MaterialInventoryValueAction;

                    break;
                case MaterialNatureEnum.MaterialNatureEnumService:
                    transToAttach.InventoryAction = transWarehouseDef.ServiceInventoryAction;
                    transToAttach.InventoryValueAction = transWarehouseDef.ServiceInventoryValueAction;
                    break;
                case MaterialNatureEnum.MaterialNatureEnumExpense:
                    transToAttach.InventoryAction = transWarehouseDef.ExpenseInventoryAction;
                    transToAttach.InventoryValueAction = transWarehouseDef.ExpenseInventoryValueAction;
                    break;
                case MaterialNatureEnum.MaterialNatureEnumIncome:
                    transToAttach.InventoryAction = transWarehouseDef.IncomeInventoryAction;
                    transToAttach.InventoryValueAction = transWarehouseDef.IncomeInventoryValueAction;
                    break;
                case MaterialNatureEnum.MaterialNatureEnumFixedAsset:
                    transToAttach.InventoryAction = transWarehouseDef.FixedAssetInventoryAction;
                    transToAttach.InventoryValueAction = transWarehouseDef.FixedAssetInventoryValueAction;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

           
            switch (transToAttach.InventoryAction)
            {
                case InventoryActionEnum.InventoryActionEnumNoChange:
                    transToAttach.TransactionType =
                        WarehouseTransactionTypeEnum.WarehouseTransactionTypeIgnore;
                    transToAttach.TransQ1 = 0;
                    transToAttach.TransQ2 = 0;
                    break;
                case InventoryActionEnum.InventoryActionEnumImport:

                    transToAttach.TransactionType =
                        WarehouseTransactionTypeEnum.WarehouseTransactionTypeImport;
                    transToAttach.TransQ1 = (decimal)transToAttach.Quontity1;
                    transToAttach.TransQ2 = (decimal)transToAttach.Quontity2;
                    break;
                case InventoryActionEnum.InventoryActionEnumExport:

                    transToAttach.TransactionType =
                        WarehouseTransactionTypeEnum.WarehouseTransactionTypeExport;
                    transToAttach.TransQ1 = (decimal)transToAttach.Quontity1;
                    transToAttach.TransQ2 = (decimal)transToAttach.Quontity2;
                    break;
                case InventoryActionEnum.InventoryActionEnumNegativeImport:

                    transToAttach.TransactionType =
                        WarehouseTransactionTypeEnum.WarehouseTransactionTypeImport;
                    transToAttach.TransQ1 = (decimal)transToAttach.Quontity1 * -1;
                    transToAttach.TransQ2 = (decimal)transToAttach.Quontity2 * -1;
                    break;
                case InventoryActionEnum.InventoryActionEnumNegativeExport:

                    transToAttach.TransactionType =
                        WarehouseTransactionTypeEnum.WarehouseTransactionTypeExport;
                    transToAttach.TransQ1 = (decimal)transToAttach.Quontity1 * -1;
                    transToAttach.TransQ2 = (decimal)transToAttach.Quontity2 * -1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            switch (transToAttach.InventoryValueAction)
            {
                case InventoryValueActionEnum.InventoryValueActionEnumNoChange:
                    transToAttach.TransNetAmount = 0;
                    transToAttach.TransFpaAmount = 0;
                    transToAttach.TransDiscountAmount = 0;
                    break;
                case InventoryValueActionEnum.InventoryValueActionEnumIncrease:
                    transToAttach.TransNetAmount = transToAttach.AmountNet;
                    transToAttach.TransFpaAmount = transToAttach.AmountFpa;
                    transToAttach.TransDiscountAmount = transToAttach.AmountDiscount;
                    break;
                case InventoryValueActionEnum.InventoryValueActionEnumDecrease:
                    transToAttach.TransNetAmount = transToAttach.AmountNet;
                    transToAttach.TransFpaAmount = transToAttach.AmountFpa;
                    transToAttach.TransDiscountAmount = transToAttach.AmountDiscount;
                    break;
                case InventoryValueActionEnum.InventoryValueActionEnumNegativeIncrease:
                    transToAttach.TransNetAmount = transToAttach.AmountNet * -1;
                    transToAttach.TransFpaAmount = transToAttach.AmountFpa * -1;
                    transToAttach.TransDiscountAmount = transToAttach.AmountDiscount * -1;

                    break;
                case InventoryValueActionEnum.InventoryValueActionEnumNegativeDecrease:
                    transToAttach.TransNetAmount = transToAttach.AmountNet * -1;
                    transToAttach.TransFpaAmount = transToAttach.AmountFpa * -1;
                    transToAttach.TransDiscountAmount = transToAttach.AmountDiscount * -1;
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
