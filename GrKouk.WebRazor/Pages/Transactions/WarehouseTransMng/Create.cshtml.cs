using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GrKouk.InfoSystem.Domain.FinConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.WebApi.Data;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Dtos.WebDtos.WarehouseTransactions;
using NToastNotify;

namespace GrKouk.WebRazor.Pages.Transactions.WarehouseTransMng
{
    public class CreateModel : PageModel
    {
        private const string SectionSystemCode = "SYS-WAREHOUSE-TRANS";

        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;
        public bool NotUpdatable;
        public bool InitialLoad = true;

        public CreateModel(GrKouk.WebApi.Data.ApiDbContext context, IMapper mapper, IToastNotification toastNotification)
        {
            _context = context;
            _mapper = mapper;
            _toastNotification = toastNotification;
        }

        public IActionResult OnGet()
        {
            LoadCombos();
            return Page();
        }

        [BindProperty]
        public WarehouseTransCreateDto ItemVm { get; set; }
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
                LoadCombos();
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
            var section = await _context.Sections.SingleOrDefaultAsync(s => s.SystemName == SectionSystemCode);
            if (section == null)
            {
                ModelState.AddModelError(string.Empty, "Δεν υπάρχει το Section");
                LoadCombos();
                return Page();
            }
            transToAttach.SectionId = section.Id;
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
            _context.WarehouseTransactions.Add(transToAttach);
            
            try
            {
                await _context.SaveChangesAsync();
               _toastNotification.AddSuccessToastMessage("Saved");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _toastNotification.AddErrorToastMessage(e.Message);
                LoadCombos();
                return Page();
            }


            return RedirectToPage("./Index");
        }
    }
}