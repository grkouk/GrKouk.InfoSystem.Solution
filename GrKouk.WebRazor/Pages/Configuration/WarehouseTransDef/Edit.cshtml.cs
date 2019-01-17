using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.Configuration.WarehouseTransDef
{
    public class EditModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public EditModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TransWarehouseDef TransWarehouseDef { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TransWarehouseDef = await _context.TransWarehouseDefs
                .Include(t => t.AmtBuyTrans)
               // .Include(t => t.AmtExportsTrans)
               // .Include(t => t.AmtImportsTrans)
                .Include(t => t.AmtInvoicedExportsTrans)
                .Include(t => t.AmtInvoicedImportsTrans)
                .Include(t => t.AmtSellTrans)
                .Include(t => t.Company)
                .Include(t => t.VolBuyTrans)
                //.Include(t => t.VolExportsTrans)
               // .Include(t => t.VolImportsTrans)
                .Include(t => t.VolInvoicedExportsTrans)
                .Include(t => t.VolInvoicedImportsTrans)
                .Include(t => t.VolSellTrans).FirstOrDefaultAsync(m => m.Id == id);

            if (TransWarehouseDef == null)
            {
                return NotFound();
            }
            LoadCombos();
            return Page();
        }
        private void LoadCombos()
        {
            var fMovements = _context.FinancialMovements.AsNoTracking().ToList();
           
            List<SelectListItem> inventoryActions = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Value = InventoryActionEnum.InventoryActionEnumNoChange.ToString(),
                    Text = "No Change"
                },
                new SelectListItem()
                {
                    Value = InventoryActionEnum.InventoryActionEnumExport.ToString(),
                    Text = "Export"
                },
                new SelectListItem()
                {
                    Value = InventoryActionEnum.InventoryActionEnumImport.ToString(),
                    Text = "Import"
                },
                new SelectListItem()
                {
                    Value = InventoryActionEnum.InventoryActionEnumNegativeExport.ToString(),
                    Text = "Neg.Export"
                },
                new SelectListItem()
                {
                    Value = InventoryActionEnum.InventoryActionEnumNegativeImport.ToString(),
                    Text = "Neg.Import"
                }
            };
            List<SelectListItem> inventoryValueActions = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Value = InventoryValueActionEnum.InventoryValueActionEnumNoChange.ToString(),
                    Text = "No Change"
                },
                new SelectListItem()
                {
                    Value = InventoryValueActionEnum.InventoryValueActionEnumDecrease.ToString(),
                    Text = "Decrease"
                },
                new SelectListItem()
                {
                    Value = InventoryValueActionEnum.InventoryValueActionEnumIncrease.ToString(),
                    Text = "Increase"
                },
                new SelectListItem()
                {
                    Value = InventoryValueActionEnum.InventoryValueActionEnumNegativeDecrease.ToString(),
                    Text = "Neg.Decrease"
                },
                new SelectListItem()
                {
                    Value = InventoryValueActionEnum.InventoryValueActionEnumNegativeIncrease.ToString(),
                    Text = "Neg.Increase"
                }
            };
            ViewData["MaterialInventoryActions"] = new SelectList(inventoryActions, "Value", "Text");
            ViewData["MaterialInventoryValueActions"] = new SelectList(inventoryValueActions, "Value", "Text");

            ViewData["ServiceInventoryAction"] = new SelectList(inventoryActions, "Value", "Text");
            ViewData["ServiceInventoryValueAction"] = new SelectList(inventoryValueActions, "Value", "Text");

            ViewData["ExpenseInventoryAction"] = new SelectList(inventoryActions, "Value", "Text");
            ViewData["ExpenseInventoryValueAction"] = new SelectList(inventoryValueActions, "Value", "Text");

            ViewData["IncomeInventoryAction"] = new SelectList(inventoryActions, "Value", "Text");
            ViewData["IncomeInventoryValueAction"] = new SelectList(inventoryValueActions, "Value", "Text");

            ViewData["FixedAssetInventoryAction"] = new SelectList(inventoryActions, "Value", "Text");
            ViewData["FixedAssetInventoryValueAction"] = new SelectList(inventoryValueActions, "Value", "Text");

            ViewData["AmtBuyTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            ViewData["AmtInvoicedExportsTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            ViewData["AmtInvoicedImportsTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            ViewData["AmtSellTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            ViewData["CompanyId"] = new SelectList(_context.Companies.AsNoTracking(), "Id", "Code");
            ViewData["VolBuyTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            ViewData["VolInvoicedExportsTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            ViewData["VolInvoicedImportsTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            ViewData["VolSellTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            var dbSeriesList = _context.TransWarehouseDocSeriesDefs.OrderBy(p => p.Name).AsNoTracking();
            List<SelectListItem> seriesList = new List<SelectListItem>();
            seriesList.Add(new SelectListItem() { Value = 0.ToString(), Text = "{No Default series}" });
            foreach (var dbSeriesItem in dbSeriesList)
            {
                seriesList.Add(new SelectListItem() { Value = dbSeriesItem.Id.ToString(), Text = dbSeriesItem.Name });
            }
            ViewData["DefaultDocSeriesId"] = new SelectList(seriesList, "Value", "Text");
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(TransWarehouseDef).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransWarehouseDefExists(TransWarehouseDef.Id))
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

        private bool TransWarehouseDefExists(int id)
        {
            return _context.TransWarehouseDefs.Any(e => e.Id == id);
        }
    }
}
